using System.ComponentModel.DataAnnotations.Schema;

namespace medirect_currency_exchange.Domain.Models
{
	public class CurrencyExchangeHistory
	{
		public Guid Id { get; set; }
		[ForeignKey("Customer")]
		public Guid CustomerId { get; set; }
		[ForeignKey("SourceWallet")]
		public Guid SourceWalletId { get; set; }
		[Column(TypeName = "decimal(18,5)")]
		public decimal SourceAmount { get; set; }
		[ForeignKey("TargetWallet")]
		public Guid TargetWalletId { get; set; }
		[Column(TypeName = "decimal(18,5)")]
		public decimal TargetAmount { get; set; }
		[Column(TypeName = "decimal(18,5)")]
		public decimal ExchangeRate { get; set; }
		public DateTime TimeStamp { get; set; }

		//Navigation
		public Customer Customer { get; set; }
		public CustomerWallet SourceWallet { get; set; }
		public CustomerWallet TargetWallet { get; set; }
	}
}