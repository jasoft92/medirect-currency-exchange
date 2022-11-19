using System.ComponentModel.DataAnnotations.Schema;

namespace medirect_currency_exchange.Models.Domain
{
	public class CustomerWallet
	{
		public Guid Id { get; set; }
		[ForeignKey("Customer")]
		public Guid CustomerId { get; set; }
		public string CurrencyCode { get; set; }
		[Column(TypeName = "decimal(18,5)")]
		public decimal Amount { get; set; }
		public DateTime LastModified { get; set; }

		//Navigation
		public Customer Customer { get; set; }
	}
}
