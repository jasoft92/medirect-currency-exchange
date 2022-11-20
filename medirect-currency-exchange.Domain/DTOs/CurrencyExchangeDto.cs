namespace medirect_currency_exchange.Domain.DTOs
{
	public class CurrencyExchangeDto
	{
		public Guid CustomerId { get; set; }
		
		public string SourceCurrency { get; set; }
		
		public string TargetCurrency { get; set; }
		
		public decimal ExchangeAmount { get; set; }
	}
}
