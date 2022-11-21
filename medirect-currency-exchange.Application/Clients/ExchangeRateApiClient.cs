using System.Net;
using Newtonsoft.Json;
using medirect_currency_exchange.Application.Clients.Models;
using medirect_currency_exchange.Contracts;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
		public async Task<Tuple<decimal?, ErrorResponse?>> GetExchangeRate(string currencyFrom, string currencyTo)
		{
			var url = _httpClient.BaseAddress + $"convert?to={currencyTo}&from={currencyFrom}&amount=1";
			var httpResponse = await _httpClient.GetAsync(url);
			var resultContent = await httpResponse.Content.ReadAsStringAsync();

			if (!httpResponse.IsSuccessStatusCode)
			{
				var error = JsonConvert.DeserializeObject<RateApiError>(resultContent);
				return new Tuple<decimal?, ErrorResponse?>(
					item1: null,
					item2: new ErrorResponse(HttpStatusCode.BadRequest, error?.ErrorDetails.Message ?? ""));
			}

			var rate = JsonConvert.DeserializeObject<RateClientResponse>(resultContent).Rate;

			return new Tuple<decimal?, ErrorResponse?>(
				item1: rate,
				item2: null);
		}
	}
}