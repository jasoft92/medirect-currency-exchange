namespace medirect_currency_exchange.Repositories
{
	public interface ICurrencyExchangeRepository
	{
		Task<int> RecordCurrencyExchangeTrade();
	}
}
