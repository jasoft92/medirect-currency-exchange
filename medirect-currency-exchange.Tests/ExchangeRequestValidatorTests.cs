using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Domain;
using medirect_currency_exchange.Validators;

namespace medirect_currency_exchange.Tests
{
	[TestFixture]
	internal class ExchangeRequestValidatorTests
	{
		[TestCaseSource(nameof(_currencyExchangeRequests))]
		public async Task GivenCurrencyExchangeRequest_ThenRequestIsInvalidOrValid(CurrencyExchangeRequest request, bool expectedResult, string errorMessage)
		{
			// Arrange
			var sut = new ExchangeRequestValidator();

			// Act
			var result = await sut.ValidateAsync(request);

			// Assert
			Assert.AreEqual(expectedResult, result != null && result.IsValid);

			if (!string.IsNullOrEmpty(errorMessage))
			{
				Assert.AreEqual(errorMessage, result?.Errors[0].ErrorMessage);
			}
		}

		private static object[] _currencyExchangeRequests =
		{
			new object[] {
				new CurrencyExchangeRequest { CustomerId = 1, ExchangeAmount = 5.5m, SourceCurrency = "EUR", TargetCurrency = "GBP"},
				true,
				string.Empty
			},
			new object[] {
				new CurrencyExchangeRequest { CustomerId = -1, ExchangeAmount = 5.5m, SourceCurrency = "EUR", TargetCurrency = "GBP"},
				false,
				ValidationErrorMessages.CustomerIdMustBeGreaterThanZero
			},
			new object[] {
				new CurrencyExchangeRequest { CustomerId = 1, ExchangeAmount = 5.5m, SourceCurrency = "", TargetCurrency = "GBP"},
				false,
				ValidationErrorMessages.SourceCurrencyCannotBeEmpty
			},
			new object[] {
				new CurrencyExchangeRequest { CustomerId = 1, ExchangeAmount = 5.5m, SourceCurrency = "EUR", TargetCurrency = ""},
				false,
				ValidationErrorMessages.TargetCurrencyCannotBeEmpty
			},
			new object[] {
				new CurrencyExchangeRequest { CustomerId = 1, ExchangeAmount = 5.5m, SourceCurrency = "EURO", TargetCurrency = "GBP"},
				false,
				ValidationErrorMessages.IncorrectSourceCurrencyFormat
			},
			new object[] {
				new CurrencyExchangeRequest { CustomerId = 1, ExchangeAmount = 5.5m, SourceCurrency = "EUR", TargetCurrency = "BP"},
				false,
				ValidationErrorMessages.IncorrectTargetCurrencyFormat
			},
			new object[] {
				new CurrencyExchangeRequest { CustomerId = 1, ExchangeAmount = -1, SourceCurrency = "EUR", TargetCurrency = "GBP"},
				false,
				ValidationErrorMessages.ExchangeAmountMustBeGreaterThanZero
			}
		};
	}
}