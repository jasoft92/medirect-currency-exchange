namespace medirect_currency_exchange.Application.Clients
{
	public interface IExchangeRateApiClient
	{ 
		Task<decimal> GetExchangeRate(string currencyFrom, string currencyTo);
	}
}