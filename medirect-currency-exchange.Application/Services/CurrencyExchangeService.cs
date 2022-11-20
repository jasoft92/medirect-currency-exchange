using medirect_currency_exchange.Application.Clients;
using medirect_currency_exchange.Database.Repositories;
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

		public async Task<decimal> ProcessExchange(CurrencyExchangeDto currencyExchangeDto)
		{
			var wallets = await _currencyExchangeRepository.GetCustomerWallets(currencyExchangeDto.CustomerId);

			var sourceWallet = wallets.SingleOrDefault(w => w.CurrencyCode == currencyExchangeDto.SourceCurrency);
			var targetWallet = wallets.SingleOrDefault(w => w.CurrencyCode == currencyExchangeDto.SourceCurrency);

			//TODO Validate - If client has wallet account with source currency
			//TODO Validate - Amount (available in client's accounts)

			var convertedAmount = await ProcessInternalAsync(
				currencyFrom: currencyExchangeDto.SourceCurrency,
				currencyTo: currencyExchangeDto.SourceCurrency,
				amount: currencyExchangeDto.ExchangeAmount);
			
			await UpdateCustomerWalletInformation(sourceWallet, currencyExchangeDto.ExchangeAmount, targetWallet, convertedAmount);
			await SaveExchangeTradeInformation(currencyExchangeDto, convertedAmount);

			return convertedAmount;
		}


		private async Task<decimal> ProcessInternalAsync(string currencyFrom, string currencyTo, decimal amount)
		{
			var key = currencyFrom + currencyTo;

			if (!_memoryCache.TryGetValue(key, out decimal exchangeRate))
			{
				var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

				exchangeRate = await _exchangeRateApiClient.GetExchangeRate(currencyFrom, currencyTo);

				_memoryCache.Set(key, exchangeRate, cacheEntryOptions);
			}

			return CalculateConversion(exchangeRate, amount);
		}

		private async Task UpdateCustomerWalletInformation(CustomerWallet sourceWallet, decimal originalAmount, CustomerWallet targetWallet, decimal convertedAmount)
		{
			sourceWallet.Amount -= originalAmount;
			targetWallet.Amount += convertedAmount;
			await _currencyExchangeRepository.SaveChangesAsync();
		}

		private async Task SaveExchangeTradeInformation(CurrencyExchangeDto currencyExchangeDto, decimal convertedAmount)
		{
			await _currencyExchangeRepository.AddCurrencyExchangeHistory(new CurrencyExchangeTransaction
			{
				CustomerId = currencyExchangeDto.CustomerId,
				FromCurrencyCode = currencyExchangeDto.SourceCurrency,
				SourceAmount = currencyExchangeDto.ExchangeAmount,
				ToCurrencyCode = currencyExchangeDto.TargetCurrency,
				ConvertedAmount = convertedAmount
			});
		}

		private decimal CalculateConversion(decimal exchangeRate, decimal amount) => amount * exchangeRate;
	}

}
