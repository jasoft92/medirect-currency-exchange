using medirect_currency_exchange.Data;
using Microsoft.EntityFrameworkCore;

namespace medirect_currency_exchange.Repositories
{
	public class CurrencyExchangeRepository : ICurrencyExchangeRepository
	{
		private readonly CurrencyExchangeDbContext _dbcontext;

		public CurrencyExchangeRepository(CurrencyExchangeDbContext dbcontext)
		{
			_dbcontext = dbcontext;
		}

		public async Task<int> RecordCurrencyExchangeTrade()
		{
			await _dbcontext.Customers.ToListAsync();
			return 1;
		}
	}
}
