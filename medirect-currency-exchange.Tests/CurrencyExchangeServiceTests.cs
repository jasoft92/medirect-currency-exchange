namespace medirect_currency_exchange.Tests
{
	internal class CurrencyExchangeServiceTests
	{
		[SetUp]
		public void Setup()
		{
		}

		public async Task GivenValidRequest_ThenExecuteSuccessfully()
		{
		}

		public async Task GivenCustomerDoesNotExist_ThenReturnError()
		{
			// Arrange

			// Act

			// Assert
			
		}

		public async Task GivenCustomerDoesNotHaveSourceCurrency_ThenReturnError()
		{
		}

		public async Task GivenCustomerDoesNotHaveTargetCurrency_ThenExecuteSuccessfully()
		{
		}

		public async Task GivenCustomerHasInsufficientFunds_ThenReturnError()
		{
		}

		public async Task GivenCustomerExceededHourlyExchangeTradeLimit_ThenReturnError()
		{
		}

		public async Task GivenSubsequentRequestsWithSameCurrencies_ThenCallExchangeRateApiOnlyOnce()
		{
		}
	}
}