using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Domain.DTOs;

namespace medirect_currency_exchange.Domain
{
	public class CurrencyExchangeProcessingResult
	{
		public ExchangeResponseDto? ExchangeResponseDto { get; }
		public ErrorResponse? ErrorResponse { get; }

		public static CurrencyExchangeProcessingResult Create(ExchangeResponseDto? exchangeResponseDto, ErrorResponse? errorResponse)
		{
			return new CurrencyExchangeProcessingResult(exchangeResponseDto, errorResponse);
		}

		private CurrencyExchangeProcessingResult(ExchangeResponseDto? exchangeResponseDto, ErrorResponse? errorResponse)
		{
			ExchangeResponseDto = exchangeResponseDto;
			ErrorResponse = errorResponse;
		}
	}
}