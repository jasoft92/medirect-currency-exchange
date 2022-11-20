using Newtonsoft.Json;
using medirect_currency_exchange.Application.Clients.Models;

namespace medirect_currency_exchange.Application.Clients
{
	public class ExchangeRateApiClient : IExchangeRateApiClient
	{
		private readonly HttpClient _httpClient;

		public ExchangeRateApiClient()
		{
			//TODO check
			_httpClient = new HttpClient
			{
				BaseAddress = new Uri("https://api.apilayer.com/exchangerates_data/"),
				DefaultRequestHeaders = { { "apikey", "WdopSdwXLg67GbYzfS2JQ8bfmIx40FfL" } }
			};
		}
		public async Task<decimal> GetExchangeRate(string currencyFrom, string currencyTo)
		{

			var url = _httpClient.BaseAddress + $"convert?to={currencyTo}&from={currencyFrom}&amount=1";
			var response = await _httpClient.GetAsync(url);
			return await ParseResponse(response);
		}

		private async Task<decimal> ParseResponse(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				//TODO Error
			}

			var resultContent = await response.Content.ReadAsStringAsync();
	
			var result = JsonConvert.DeserializeObject<RateClientResponse>(resultContent);
			return result?.Rate ?? -1;
		}
	}
}