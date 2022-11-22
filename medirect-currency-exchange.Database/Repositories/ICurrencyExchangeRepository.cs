using medirect_currency_exchange.Domain.Models;

namespace medirect_currency_exchange.Database.Repositories
{
	public interface ICurrencyExchangeRepository
	{
		Task<Customer?> GetCustomer(long customerId);
		Task<List<CustomerWallet?>> GetCustomerWallets(long customerId);
		Task<CustomerWallet> AddCustomerWallet(CustomerWallet newCustomerWallet);
		Task<CurrencyExchangeTransaction> AddCurrencyExchangeHistory(CurrencyExchangeTransaction currencyExchangeTransaction);
		Task<List<CurrencyExchangeTransaction>> GetRecentCurrencyExchangeTransactions(long customerId);
		Task SaveChangesAsync();
	}
}
