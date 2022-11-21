using medirect_currency_exchange.Domain.Models;

namespace medirect_currency_exchange.Database.Repositories
{
	public interface ICurrencyExchangeRepository
	{
		Task<List<CustomerWallet?>> GetCustomerWallets(double customerId);
		Task<CurrencyExchangeTransaction> AddCurrencyExchangeHistory(CurrencyExchangeTransaction currencyExchangeTransaction);
		Task SaveChangesAsync();
	}
}
