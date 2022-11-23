using System.Net;
using Newtonsoft.Json;
using medirect_currency_exchange.Application.Clients.Models;
using medirect_currency_exchange.Application.Exception;
using medirect_currency_exchange.Logger;

namespace medirect_currency_exchange.Application.Clients
{
	public class ExchangeRateApiClient : IExchangeRateApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly ILoggerManager _loggerManager;

		private const string INVALID_FROM_CURRENCY = "invalid_from_currency";
		private const string INVALID_TO_CURRENCY = "invalid_to_currency";
		private const string INVALID_AMOUNT = "invalid_conversion_amount";

		public ExchangeRateApiClient(ILoggerManager loggerManager, HttpClient httpClient)
		{
			_loggerManager = loggerManager;
			_httpClient = httpClient;
		}

		public async Task<decimal> GetExchangeRate(string currencyFrom, string currencyTo)
		{
			var url = _httpClient.BaseAddress + $"convert?to={currencyTo}&from={currencyFrom}&amount=1";
			var httpResponse = await _httpClient.GetAsync(url);

			var resultContent = await httpResponse.Content.ReadAsStringAsync();

			if (!httpResponse.IsSuccessStatusCode)
			{
				var error = JsonConvert.DeserializeObject<RateApiError>(resultContent);

				switch (error?.ErrorDetails?.Code)
				{
					case INVALID_FROM_CURRENCY:
						throw new ApiException(HttpStatusCode.BadRequest, "The currency to convert FROM is invalid");

					case INVALID_TO_CURRENCY:
						throw new ApiException(HttpStatusCode.BadRequest, "The currency to convert TO is invalid");

					case INVALID_AMOUNT:
						throw new ApiException(HttpStatusCode.BadRequest, "The amount to be converted is invalid");
				}

				throw new ApiException(HttpStatusCode.InternalServerError, "Error while retrieving currency exchange rate");
			}

			var rateClientResponse = JsonConvert.DeserializeObject<RateClientResponse>(resultContent);
			if (rateClientResponse != null)
			{
				_loggerManager.LogInfo($"Exchange Rate API Request. SourceCurrency: {currencyFrom} | TargetCurrency {currencyTo} | Rate: {rateClientResponse.Rate}");
				return rateClientResponse.Rate;
			}

			throw new ApiException(HttpStatusCode.InternalServerError, "Error with currency exchange rate retrieval");
		}
	}
}