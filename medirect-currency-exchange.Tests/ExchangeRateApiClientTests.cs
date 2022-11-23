using medirect_currency_exchange.Application.Clients;
using medirect_currency_exchange.Logger;
using Moq;
using System.Net;
using medirect_currency_exchange.Application.Exception;
using medirect_currency_exchange.Domain;
using RichardSzalay.MockHttp;

namespace medirect_currency_exchange.Tests
{
	[TestFixture]
	internal class ExchangeRateApiClientTests
	{
		private string mockUrl = "https://mockurl.com/";
		private MockHttpMessageHandler mockHttp;

		[SetUp]
		public void Setup()
		{
			mockHttp = new MockHttpMessageHandler();
		}

		[Test]
		public async Task GivenValidRequest_ThenReturnCorrectExchangeRate()
		{
			//Arrange
			const decimal expectedRate = 4.3456m;

			mockHttp
				.When($"{mockUrl}*")
				.Respond(HttpStatusCode.OK, "application/json", "{\"date\":\"2022-11-23\",\"info\":{\"rate\":0.864904,\"timestamp\":1669201503},\"query\":{\"amount\":5,\"from\":\"EUR\",\"to\":\"GBP\"},\"result\":" + expectedRate + ",\"success\":true}");

			// Inject the handler or client into your application code
			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri(mockUrl);

			//Act
			var erap = new ExchangeRateApiClient(new Mock<ILoggerManager>().Object, client);
			//var sut = async () => await erap.GetExchangeRate("EUR", "USD");
			var actualRate = await erap.GetExchangeRate("EUR", "USD");

			//Assert
			Assert.AreEqual(expectedRate, actualRate);
		}

		[Test]
		public async Task GivenInvalidFromCurrency_ThenThrowApiException()
		{
			//Arrange
			mockHttp
				.When($"{mockUrl}*")
				.Respond(HttpStatusCode.BadRequest, "application/json", "{\"error\":{\"code\":\"invalid_from_currency\"}}");

			// Inject the handler or client into your application code
			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri(mockUrl);

			//Act
			var erap = new ExchangeRateApiClient(new Mock<ILoggerManager>().Object, client);
			var sut = async () => await erap.GetExchangeRate("EUR", "USD");

			//Assert
			var ex = Assert.ThrowsAsync<ApiException>(() => sut.Invoke());
			Assert.AreEqual(HttpStatusCode.BadRequest, ex.HttpStatusCode);
			Assert.AreEqual(ValidationErrorMessages.InvalidFromCurrency, ex.Message);
		}

		[Test]
		public async Task GivenInvalidToCurrency_ThenThrowApiException()
		{
			//Arrange
			mockHttp
				.When($"{mockUrl}*")
				.Respond(HttpStatusCode.BadRequest, "application/json", "{\"error\":{\"code\":\"invalid_to_currency\"}}");

			// Inject the handler or client into your application code
			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri(mockUrl);

			//Act
			var erap = new ExchangeRateApiClient(new Mock<ILoggerManager>().Object, client);
			var sut = async () => await erap.GetExchangeRate("EUR", "USD");

			//Assert
			var ex = Assert.ThrowsAsync<ApiException>(() => sut.Invoke());
			Assert.AreEqual(HttpStatusCode.BadRequest, ex.HttpStatusCode);
			Assert.AreEqual(ValidationErrorMessages.InvalidToCurrency, ex.Message);
		}

		[Test]
		public async Task GivenInvalidAmount_ThenThrowApiException()
		{
			//Arrange
			mockHttp
				.When($"{mockUrl}*")
				.Respond(HttpStatusCode.BadRequest, "application/json", "{\"error\":{\"code\":\"invalid_conversion_amount\"}}");

			// Inject the handler or client into your application code
			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri(mockUrl);

			//Act
			var erap = new ExchangeRateApiClient(new Mock<ILoggerManager>().Object, client);
			var sut = async () => await erap.GetExchangeRate("EUR", "USD");

			//Assert
			var ex = Assert.ThrowsAsync<ApiException>(() => sut.Invoke());
			Assert.AreEqual(HttpStatusCode.BadRequest, ex.HttpStatusCode);
			Assert.AreEqual(ValidationErrorMessages.InvalidAmountToConvert, ex.Message);
		}

		[Test]
		public async Task GivenOtherApiError_ThenThrowApiException()
		{
			//Arrange
			mockHttp
				.When($"{mockUrl}*")
				.Respond(HttpStatusCode.BadRequest, "application/json", "{\"error\":{\"code\":\"unhandled_error_code\"}}");

			// Inject the handler or client into your application code
			var client = mockHttp.ToHttpClient();
			client.BaseAddress = new Uri(mockUrl);

			//Act
			var erap = new ExchangeRateApiClient(new Mock<ILoggerManager>().Object, client);
			var sut = async () => await erap.GetExchangeRate("EUR", "USD");

			//Assert
			var ex = Assert.ThrowsAsync<ApiException>(() => sut.Invoke());
			Assert.AreEqual(HttpStatusCode.InternalServerError, ex.HttpStatusCode);
			Assert.AreEqual(ValidationErrorMessages.GenericApiError, ex.Message);
		}
	}
}
