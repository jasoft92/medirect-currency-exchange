namespace medirect_currency_exchange.Domain.Models
{
	public class Customer
	{
		public double Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string IdCard { get; set; }
		public DateTime DOB { get; set; }
		public string Email { get; set; }

		
		public IEnumerable<CustomerWallet> Wallets { get; set; }
	}
}
