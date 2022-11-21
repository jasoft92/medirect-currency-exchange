using System.ComponentModel.DataAnnotations.Schema;

namespace medirect_currency_exchange.Domain.Models
{
	public class CurrencyExchangeTransaction
	{
		public Guid Id { get; set; }
		[ForeignKey("Customer")]
		public double CustomerId { get; set; }
		public string FromCurrencyCode { get; set; }
		[Column(TypeName = "decimal(18,5)")]
		public decimal SourceAmount { get; set; }
		public string ToCurrencyCode { get; set; }
		[Column(TypeName = "decimal(18,5)")]
		public decimal ConvertedAmount { get; set; }
		[Column(TypeName = "decimal(18,5)")]
		public decimal ExchangeRate { get; set; }
		public DateTime TimeStamp { get; set; }


		public Customer Customer { get; set; }
	}
}
