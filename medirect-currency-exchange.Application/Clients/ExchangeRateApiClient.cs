using System.Net;
using Newtonsoft.Json;
using medirect_currency_exchange.Application.Clients.Models;
using medirect_currency_exchange.Application.Exception;

namespace medirect_currency_exchange.Application.Clients
{
	public class ExchangeRateApiClient : IExchangeRateApiClient
	{
		private readonly HttpClient _httpClient;

		private const string INVALID_FROM_CURRENCY = "invalid_from_currency";
		private const string INVALID_TO_CURRENCY = "invalid_to_currency";
		private const string INVALID_AMOUNT = "invalid_conversion_amount";

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
					case INVALID_FROM_CURRENCY:
						throw new ApiException(HttpStatusCode.BadRequest, "The currency to convert FROM is invalid");

					case INVALID_TO_CURRENCY:
						throw new ApiException(HttpStatusCode.BadRequest, "The currency to convert TO is invalid");
					
					case INVALID_AMOUNT:
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