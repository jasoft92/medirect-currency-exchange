using System.Net;

namespace medirect_currency_exchange.Domain.DTOs
{
	public class ErrorResponseDto
	{
		public HttpStatusCode ErrorCode { get; set; }

		public string Message { get; set; }

		public ErrorResponseDto(HttpStatusCode errorCode, string message)
		{
			ErrorCode = errorCode;
			Message = message;
		}
	}
}
