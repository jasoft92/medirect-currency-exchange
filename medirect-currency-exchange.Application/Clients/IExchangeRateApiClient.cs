using medirect_currency_exchange.Contracts;

namespace medirect_currency_exchange.Application.Clients
{
	public interface IExchangeRateApiClient
	{
		Task<Tuple<decimal?, ErrorResponse?>> GetExchangeRate(string currencyFrom, string currencyTo);
	}
}