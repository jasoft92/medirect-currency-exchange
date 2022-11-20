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

		public async Task<List<CustomerWallet?>> GetCustomerWallets(Guid customerId)
		{
			return await _dbcontext.CustomerWallets.Where(cw => cw.CustomerId == customerId).ToListAsync();
		}

		public async Task<CurrencyExchangeTransaction> AddCurrencyExchangeHistory(CurrencyExchangeTransaction currencyExchangeTransaction)
		{
			await _dbcontext.AddAsync(currencyExchangeTransaction);
			await _dbcontext.SaveChangesAsync();
			return currencyExchangeTransaction;
		}

		public async Task SaveChangesAsync()
		{
			await _dbcontext.SaveChangesAsync();
		}
	}
}
