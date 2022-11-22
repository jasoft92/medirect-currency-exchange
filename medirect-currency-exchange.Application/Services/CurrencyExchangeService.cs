﻿using System.Net;
using medirect_currency_exchange.Application.Clients;
using medirect_currency_exchange.Database.Repositories;
using medirect_currency_exchange.Domain;
using medirect_currency_exchange.Domain.DTOs;
using medirect_currency_exchange.Domain.Models;
using medirect_currency_exchange.Logger;
using Microsoft.Extensions.Caching.Memory;

namespace medirect_currency_exchange.Application.Services
{
	public class CurrencyExchangeService : ICurrencyExchangeService
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IExchangeRateApiClient _exchangeRateApiClient;
		private readonly ICurrencyExchangeRepository _currencyExchangeRepository;
		private readonly ILoggerManager _loggerManager;

		public CurrencyExchangeService(IMemoryCache memoryCache, IExchangeRateApiClient exchangeRateApiClient, ICurrencyExchangeRepository currencyExchangeRepository, ILoggerManager loggerManager)
		{
			_memoryCache = memoryCache;
			_exchangeRateApiClient = exchangeRateApiClient;
			_currencyExchangeRepository = currencyExchangeRepository;
			_loggerManager = loggerManager;
		}

		public async Task<CurrencyExchangeProcessingResult> ProcessExchange(ExchangeRequestDto exchangeRequestDto)
		{
			var wallets = await _currencyExchangeRepository.GetCustomerWallets(exchangeRequestDto.CustomerId);

			var sourceWallet = wallets.SingleOrDefault(w => w.CurrencyCode == exchangeRequestDto.SourceCurrency);
			var targetWallet = wallets.SingleOrDefault(w => w.CurrencyCode == exchangeRequestDto.TargetCurrency);

			var validationErrors = await ValidateRequest(exchangeRequestDto, sourceWallet);

			if (validationErrors != null)
				return CurrencyExchangeProcessingResult.Create(null, validationErrors);

			var exchangeRate = await GetExchangeRate(
				currencyFrom: exchangeRequestDto.SourceCurrency,
				currencyTo: exchangeRequestDto.TargetCurrency);

			if (targetWallet == null)
			{
				targetWallet = await _currencyExchangeRepository.AddCustomerWallet(new CustomerWallet
				{
					CustomerId = exchangeRequestDto.CustomerId,
					Amount = 0,
					CurrencyCode = exchangeRequestDto.TargetCurrency
				});
				_loggerManager.LogInfo($"Created wallet for {exchangeRequestDto.TargetCurrency} currency for customer {exchangeRequestDto.CustomerId}");
			}

			var convertedAmount = CalculateConversion(exchangeRate, exchangeRequestDto.ExchangeAmount);

			await UpdateCustomerWalletInformation(sourceWallet, exchangeRequestDto.ExchangeAmount, targetWallet, convertedAmount);
			await SaveExchangeTradeInformation(exchangeRequestDto, exchangeRate, convertedAmount);

			return CurrencyExchangeProcessingResult.Create(new ExchangeResponseDto(
					customerId: exchangeRequestDto.CustomerId,
					sourceAccountBalance: sourceWallet.Amount,
					sourceCurrencyCode: sourceWallet.CurrencyCode,
					targetAccountBalance: targetWallet.Amount,
					targetCurrencyCode: targetWallet.CurrencyCode,
					exchangeAmount: convertedAmount), null);
		}


		private async Task<ErrorResponseDto?> ValidateRequest(ExchangeRequestDto exchangeRequestDto, CustomerWallet? customerWallet)
		{
			var customer = await _currencyExchangeRepository.GetCustomer(exchangeRequestDto.CustomerId);

			if (customer == null)
			{
				return LogAndReturnError(HttpStatusCode.BadRequest, "Customer does not exist");
			}

			if (customerWallet == null)
			{
				return LogAndReturnError(HttpStatusCode.BadRequest, $"Invalid Request. Client does not have an account with {exchangeRequestDto.SourceCurrency} currency");
			}

			if (customerWallet.Amount < exchangeRequestDto.ExchangeAmount)
			{
				return LogAndReturnError(HttpStatusCode.UnprocessableEntity, $"Client has insufficient funds in his {exchangeRequestDto.SourceCurrency} account to perform the requested exchange");
			}

			var recentExchangeTrades = await _currencyExchangeRepository.GetRecentCurrencyExchangeTransactions(exchangeRequestDto.CustomerId);
			return recentExchangeTrades.Count >= 10 ? LogAndReturnError(HttpStatusCode.UnprocessableEntity, "Client exceeded maximum allowed exchange trades per hour") : null;
		}

		private ErrorResponseDto? LogAndReturnError(HttpStatusCode statusCode, string errorMessage)
		{
			_loggerManager.LogError(errorMessage);
			return new ErrorResponseDto(statusCode, errorMessage);
		}

		private async Task<decimal> GetExchangeRate(string currencyFrom, string currencyTo)
		{
			var key = currencyFrom + currencyTo;

			if (!_memoryCache.TryGetValue(key, out decimal exchangeRate))
			{
				var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

				exchangeRate = await _exchangeRateApiClient.GetExchangeRate(currencyFrom, currencyTo);

				_memoryCache.Set(key, exchangeRate, cacheEntryOptions);
				_loggerManager.LogDebug($"Updated mem cache with {key} key. Exchange rate: {exchangeRate}.");
			}

			return exchangeRate;
		}

		private async Task UpdateCustomerWalletInformation(CustomerWallet sourceWallet, decimal originalAmount, CustomerWallet targetWallet, decimal convertedAmount)
		{
			var dateNow = DateTime.Now;
			sourceWallet.Amount -= originalAmount;
			sourceWallet.LastModified = dateNow;
			targetWallet.Amount += convertedAmount;
			targetWallet.LastModified = dateNow;
			await _currencyExchangeRepository.SaveChangesAsync();
		}

		private async Task SaveExchangeTradeInformation(ExchangeRequestDto currencyExchangeDto, decimal exchangeRate, decimal convertedAmount)
		{
			await _currencyExchangeRepository.AddCurrencyExchangeHistory(new CurrencyExchangeTransaction
			{
				CustomerId = currencyExchangeDto.CustomerId,
				FromCurrencyCode = currencyExchangeDto.SourceCurrency,
				SourceAmount = currencyExchangeDto.ExchangeAmount,
				ToCurrencyCode = currencyExchangeDto.TargetCurrency,
				ConvertedAmount = convertedAmount,
				ExchangeRate = exchangeRate,
				TimeStamp = DateTime.Now
			});
		}

		private decimal CalculateConversion(decimal exchangeRate, decimal amount) => amount * exchangeRate;
	}
}