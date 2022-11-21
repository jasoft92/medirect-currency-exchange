using System.Runtime.Serialization;

namespace medirect_currency_exchange.Contracts
{
	[DataContract]
	public class CurrencyExchangeResponse
	{
		[DataMember]
		public double CustomerId { get; set; }

		[DataMember]
		public decimal SourceAccountBalance { get; set; }

		[DataMember]
		public string SourceCurrencyCode { get; set; }

		[DataMember]
		public decimal TargetAccountBalance { get; set; }

		[DataMember]
		public string TargetCurrencyCode { get; set; }

		[DataMember]
		public decimal ExchangeAmount { get; set; }
	}
}