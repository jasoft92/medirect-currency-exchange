using medirect_currency_exchange.Database.Context;
using medirect_currency_exchange.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace medirect_currency_exchange.Database.Repositories
{
	public class CurrencyExchangeRepository : ICurrencyExchangeRepository
	{
		private readonly CurrencyExchangeDbContext _dbcontext;

		public CurrencyExchangeRepository(CurrencyExchangeDbContext dbcontext)
		{
			_dbcontext = dbcontext;
		}

		public Task<Customer?> GetCustomer(long customerId)
		{
			return _dbcontext.Customers.SingleOrDefaultAsync(cw => cw.Id == customerId);
		}

		public Task<List<CustomerWallet?>> GetCustomerWallets(long customerId)
		{
			return _dbcontext.CustomerWallets.Where(cw => cw.CustomerId == customerId).ToListAsync();
		}

		public async Task<CustomerWallet> AddCustomerWallet(CustomerWallet newCustomerWallet)
		{
			await _dbcontext.CustomerWallets.AddAsync(newCustomerWallet);
			await _dbcontext.SaveChangesAsync();
			return newCustomerWallet;
		}

		public async Task<CurrencyExchangeTransaction> AddCurrencyExchangeHistory(CurrencyExchangeTransaction currencyExchangeTransaction)
		{
			await _dbcontext.AddAsync(currencyExchangeTransaction);
			await _dbcontext.SaveChangesAsync();
			return currencyExchangeTransaction;
		}

		public Task<List<CurrencyExchangeTransaction>> GetRecentCurrencyExchangeTransactions(long customerId)
		{
			return _dbcontext.CurrencyExchangeTransactions.Where(t => t.CustomerId == customerId && t.TimeStamp > DateTime.Now.AddHours(-1)).ToListAsync();
		}

		public async Task SaveChangesAsync()
		{
			await _dbcontext.SaveChangesAsync();
		}
	}
}
