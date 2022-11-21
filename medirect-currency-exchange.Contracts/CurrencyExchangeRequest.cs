using System.Runtime.Serialization;

namespace medirect_currency_exchange.Contracts
{
	[DataContract]
	public class CurrencyExchangeRequest
	{
		[DataMember]
		public double CustomerId { get; set; }

		[DataMember]
		public string SourceCurrency { get; set; }

		[DataMember] 
		public string TargetCurrency { get; set; }

		[DataMember]
		public decimal ExchangeAmount { get; set; }
	}
}