using medirect_currency_exchange.Database.Context;

namespace medirect_currency_exchange.Database.Repositories
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
			//await _dbcontext.Customers.ToListAsync();
			return 1;
		}
	}
}
