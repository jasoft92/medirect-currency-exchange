using medirect_currency_exchange.Application.Clients;
using Microsoft.Extensions.Caching.Memory;

namespace medirect_currency_exchange.Application.Services
{
	public class CurrencyExchangeService
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IExchangeRateApiClient _exchangeRateApiClient;

		public CurrencyExchangeService(IMemoryCache memoryCache, IExchangeRateApiClient exchangeRateApiClient)
		{
			_memoryCache = memoryCache;
			_exchangeRateApiClient = exchangeRateApiClient;
		}

		public async Task ProcessExchange(string currencyFrom, string currencyTo, decimal amount)
		{
			var key = currencyFrom + currencyTo;

			if (!_memoryCache.TryGetValue(key, out decimal exchangeRate))
			{
				var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

				exchangeRate = await _exchangeRateApiClient.GetExchangeRate(currencyFrom, currencyTo);

				_memoryCache.Set(key, exchangeRate, cacheEntryOptions);
			}

			var convertedAmount = CalculateConversion(exchangeRate, amount);

		}

		private decimal CalculateConversion(decimal exchangeRate, decimal amount) => amount * exchangeRate;
	}

}
