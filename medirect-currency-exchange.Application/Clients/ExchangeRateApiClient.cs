using System.Net;
using Newtonsoft.Json;
using medirect_currency_exchange.Application.Clients.Models;
using medirect_currency_exchange.Application.Exception;
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
		public async Task<decimal> GetExchangeRate(string currencyFrom, string currencyTo)
		{
			var url = _httpClient.BaseAddress + $"convert?to={currencyTo}&from={currencyFrom}&amount=1";
			var httpResponse = await _httpClient.GetAsync(url);
			var resultContent = await httpResponse.Content.ReadAsStringAsync();

			if (!httpResponse.IsSuccessStatusCode)
			{
				var error = JsonConvert.DeserializeObject<RateApiError>(resultContent);
				
				switch (error?.ErrorDetails.Code)
				{
					case "invalid_from_currency":
						throw new ApiException(HttpStatusCode.BadRequest, "The currency to convert FROM is invalid");

					case "invalid_to_currency":
						throw new ApiException(HttpStatusCode.BadRequest, "The currency to convert TO is invalid");
					
					case "invalid_conversion_amount":
						throw new ApiException(HttpStatusCode.BadRequest, "The amount to be converted is invalid");

					default: throw new ApiException(HttpStatusCode.InternalServerError, error?.ErrorDetails.Message ?? "Error with currency exchange rate retrieval");
				}
			}

			var rateClientResponse = JsonConvert.DeserializeObject<RateClientResponse>(resultContent);
			if (rateClientResponse != null) return rateClientResponse.Rate;

			throw new ApiException(HttpStatusCode.InternalServerError, "Error with currency exchange rate retrieval");
		}
	}
}