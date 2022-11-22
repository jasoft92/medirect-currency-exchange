using medirect_currency_exchange.Domain.DTOs;

namespace medirect_currency_exchange.Domain
{
	public class CurrencyExchangeProcessingResult
	{
		public ExchangeResponseDto? ExchangeResponseDto { get; }
		public ErrorResponseDto? ErrorResponse { get; }

		public static CurrencyExchangeProcessingResult Create(ExchangeResponseDto? exchangeResponseDto, ErrorResponseDto? errorResponse)
		{
			return new CurrencyExchangeProcessingResult(exchangeResponseDto, errorResponse);
		}

		private CurrencyExchangeProcessingResult(ExchangeResponseDto? exchangeResponseDto, ErrorResponseDto? errorResponse)
		{
			ExchangeResponseDto = exchangeResponseDto;
			ErrorResponse = errorResponse;
		}
	}
}