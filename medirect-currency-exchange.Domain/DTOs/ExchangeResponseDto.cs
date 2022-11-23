namespace medirect_currency_exchange.Domain.DTOs
{
	public class ExchangeResponseDto
	{
		public long CustomerId { get; }

		public decimal SourceAccountBalance { get; }

		public string SourceCurrencyCode { get; }

		public decimal TargetAccountBalance { get; }

		public string TargetCurrencyCode { get; }

		public decimal ExchangeAmount { get; }

		public decimal ExchangeRate { get; }

		public ExchangeResponseDto(long customerId, decimal sourceAccountBalance, string sourceCurrencyCode, decimal targetAccountBalance, string targetCurrencyCode, decimal exchangeAmount, decimal exchangeRate)
		{
			CustomerId = customerId;
			SourceAccountBalance = sourceAccountBalance;
			SourceCurrencyCode = sourceCurrencyCode;
			TargetAccountBalance = targetAccountBalance;
			TargetCurrencyCode = targetCurrencyCode;
			ExchangeAmount = exchangeAmount;
			ExchangeRate = exchangeRate;
		}
	}
}
