namespace medirect_currency_exchange.Database.Repositories
{
	public interface ICurrencyExchangeRepository
	{
		Task<int> RecordCurrencyExchangeTrade();
	}
}
