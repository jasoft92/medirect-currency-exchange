using medirect_currency_exchange.Application.Clients;
using medirect_currency_exchange.Logger;
using Moq;
using System.Net;
using RichardSzalay.MockHttp;

namespace medirect_currency_exchange.Tests
{
	internal class ExchangeRateApiClientTests
	{
		private Mock<HttpClient> httpClientMock;

		[SetUp]
		public void Setup()
		{
			httpClientMock = new Mock<HttpClient>();
		}

		public async Task GivenValidRequest_ThenExecuteSuccessfully()
		{
		}

		[Test]
		public async Task GivenInvalidFromCurrency_ThenThrowApiException()
		{
			var mockHttp = new MockHttpMessageHandler();
			
			mockHttp
				.When("https://api.apilayer.com/exchangerates_data/*")
				.Respond(HttpStatusCode.BadRequest, "application/json", "{\"error\":{\"code\":\"invalid_from_currency\"}}"); // Respond with JSON

			// Inject the handler or client into your application code
			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri("https://api.apilayer.com/exchangerates_data/");


			ExchangeRateApiClient erap = new ExchangeRateApiClient(new Mock<ILoggerManager>().Object, client);
			var sut = await erap.GetExchangeRate("EUR", "USD");
		}

		public async Task GivenInvalidToCurrency_ThenThrowApiException()
		{
		}

		public async Task GivenInvalidAmount_ThenThrowApiException()
		{
		}
	}
}
