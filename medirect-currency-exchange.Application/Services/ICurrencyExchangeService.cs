using medirect_currency_exchange.Domain;
using medirect_currency_exchange.Domain.DTOs;

namespace medirect_currency_exchange.Application.Services
{
	public interface ICurrencyExchangeService
	{
		Task<CurrencyExchangeProcessingResult> ProcessExchange(ExchangeRequestDto currencyExchangeDto);
	}
}