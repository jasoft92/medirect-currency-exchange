using System.Net;
using medirect_currency_exchange.Application.Clients;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Database.Repositories;
using medirect_currency_exchange.Domain;
using medirect_currency_exchange.Domain.DTOs;
using medirect_currency_exchange.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace medirect_currency_exchange.Application.Services
{
	public class CurrencyExchangeService : ICurrencyExchangeService
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IExchangeRateApiClient _exchangeRateApiClient;
		private readonly ICurrencyExchangeRepository _currencyExchangeRepository;

		public CurrencyExchangeService(IMemoryCache memoryCache, IExchangeRateApiClient exchangeRateApiClient, ICurrencyExchangeRepository currencyExchangeRepository)
		{
			_memoryCache = memoryCache;
			_exchangeRateApiClient = exchangeRateApiClient;
			_currencyExchangeRepository = currencyExchangeRepository;
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

			//TODO ValidateExchangeRate Validity

			targetWallet ??= await _currencyExchangeRepository.AddCustomerWallet(new CustomerWallet
			{
				CustomerId = exchangeRequestDto.CustomerId,
				Amount = 0,
				CurrencyCode = exchangeRequestDto.TargetCurrency
			});

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


		private async Task<ErrorResponse?> ValidateRequest(ExchangeRequestDto exchangeRequestDto, CustomerWallet? customerWallet)
		{
			if (customerWallet == null)
			{
				return new ErrorResponse(HttpStatusCode.BadRequest, $"Invalid Request. Client does not have an account with {exchangeRequestDto.SourceCurrency} currency.");
			}

			if (customerWallet.Amount < exchangeRequestDto.ExchangeAmount)
			{
				return new ErrorResponse(HttpStatusCode.UnprocessableEntity, $"Client has insufficient funds in his {exchangeRequestDto.SourceCurrency} account to perform the requested exchange.");
			}

			var recentExchangeTrades = await _currencyExchangeRepository.GetRecentCurrencyExchangeTransactions(exchangeRequestDto.CustomerId);
			return recentExchangeTrades.Count >= 10 ? new ErrorResponse(HttpStatusCode.UnprocessableEntity, "Client exceeded maximum allowed exchange trades per hour.") : null;
		}

		private async Task<decimal> GetExchangeRate(string currencyFrom, string currencyTo)
		{
			var key = currencyFrom + currencyTo;

			if (!_memoryCache.TryGetValue(key, out decimal exchangeRate))
			{
				var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

				var exchangeRateApiResponse = await _exchangeRateApiClient.GetExchangeRate(currencyFrom, currencyTo);

				if (exchangeRateApiResponse.Item1 > 0 && exchangeRateApiResponse.Item2 == null)
				{
					exchangeRate = exchangeRateApiResponse.Item1.Value;
					_memoryCache.Set(key, exchangeRate, cacheEntryOptions);
				}
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