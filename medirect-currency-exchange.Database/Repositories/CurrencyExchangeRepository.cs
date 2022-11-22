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

		public async Task<Customer?> GetCustomer(long customerId)
		{
			return await _dbcontext.Customers.SingleOrDefaultAsync(cw => cw.Id == customerId);
		}

		public async Task<List<CustomerWallet?>> GetCustomerWallets(long customerId)
		{
			return await _dbcontext.CustomerWallets.Where(cw => cw.CustomerId == customerId).ToListAsync();
		}

		public async Task<CustomerWallet> AddCustomerWallet(CustomerWallet newCustomerWallet)
		{
			_dbcontext.CustomerWallets.Add(newCustomerWallet);
			await _dbcontext.SaveChangesAsync();
			return newCustomerWallet;
		}

		public async Task<CurrencyExchangeTransaction> AddCurrencyExchangeHistory(CurrencyExchangeTransaction currencyExchangeTransaction)
		{
			await _dbcontext.AddAsync(currencyExchangeTransaction);
			await _dbcontext.SaveChangesAsync();
			return currencyExchangeTransaction;
		}

		public async Task<List<CurrencyExchangeTransaction>> GetRecentCurrencyExchangeTransactions(long customerId)
		{
			return await _dbcontext.CurrencyExchangeTransactions.Where(t => t.CustomerId == customerId && t.TimeStamp > DateTime.Now.AddHours(-1)).ToListAsync();
		}

		public async Task SaveChangesAsync()
		{
			await _dbcontext.SaveChangesAsync();
		}
	}
}
