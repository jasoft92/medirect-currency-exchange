using System.Runtime.Serialization;

namespace medirect_currency_exchange.Contracts
{
	public class CurrencyExchangeRequest
	{
		[DataMember(Name = "customerId")]
		public double CustomerId { get; set; }

		[DataMember(Name = "sourceCurrency")]
		public string SourceCurrency { get; set; }

		[DataMember(Name = "targetCurrency")] 
		public string TargetCurrency { get; set; }

		[DataMember(Name = "exchangeAmount")]
		public decimal ExchangeAmount { get; set; }
	}
}