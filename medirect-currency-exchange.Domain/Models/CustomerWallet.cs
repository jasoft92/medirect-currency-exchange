using System.ComponentModel.DataAnnotations.Schema;

namespace medirect_currency_exchange.Domain.Models
{
	public class CustomerWallet
	{
		public long CustomerId { get; set; }

		public string CurrencyCode { get; set; }
		
		[Column(TypeName = "decimal(18,5)")]
		public decimal Amount { get; set; }

		public DateTime LastModified { get; set; }

		public Customer Customer { get; set; }
	}
}