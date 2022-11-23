namespace medirect_currency_exchange.Domain
{
	public static class ValidationErrorMessages
	{
		//RequestValidationErrors
		public const string CustomerIdCannotBeEmpty = "CustomerID cannot be empty";
		public const string CustomerIdMustBeGreaterThanZero = "CustomerID must be greater than 0";
		public const string SourceCurrencyCannotBeEmpty = "Source currency code cannot be empty";
		public const string IncorrectSourceCurrencyFormat = "Incorrect source currency format, currency code must be 3 characters long";
		public const string TargetCurrencyCannotBeEmpty = "Target currency code cannot be empty";
		public const string IncorrectTargetCurrencyFormat = "Incorrect target currency format, currency code must be 3 characters long";
		public const string ExchangeAmountCannotBeEmpty = "Exchange amount cannot be empty";
		public const string ExchangeAmountMustBeGreaterThanZero = "Exchange amount must be greater than 0";

		//Service Errors
		public const string CustomerDoesNotExist = "Customer does not exist";
		public const string CurrencyAccountNotFound = "Invalid Request. Client does not have an account with {0} currency";
		public const string InsufficientFunds = "Client has insufficient funds in his {0} account to perform the requested exchange";
		public const string CustomerExceededHourlyTradeLimit = "Client exceeded maximum allowed exchange trades per hour";

		//ExchangeRate API Errors
		public const string InvalidFromCurrency = "The currency to convert FROM is invalid";
		public const string InvalidToCurrency = "The currency to convert TO is invalid";
		public const string InvalidAmountToConvert = "The amount to be converted is invalid";
		public const string GenericApiError = "Error while retrieving currency exchange rate";
	}
}
