namespace medirect_currency_exchange.Domain.DTOs
{
	public class ExchangeRequestDto
	{
		public long CustomerId { get; }

		public string SourceCurrency { get; }

		public string TargetCurrency { get; }

		public decimal ExchangeAmount { get; }
		
		public ExchangeRequestDto(long customerId, string sourceCurrency, string targetCurrency, decimal exchangeAmount)
		{
			CustomerId = customerId;
			SourceCurrency = sourceCurrency;
			TargetCurrency = targetCurrency;
			ExchangeAmount = exchangeAmount;
		}
	}
}
