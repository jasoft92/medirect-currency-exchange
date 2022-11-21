using System.Net;

namespace medirect_currency_exchange.Contracts
{
	public class ErrorResponse
	{
		public HttpStatusCode ErrorCode { get; set; }

		public string Message { get; set; }

		public ErrorResponse(HttpStatusCode errorCode)
		{
			ErrorCode = errorCode;
		}

		public ErrorResponse(HttpStatusCode errorCode, string message)
		{
			ErrorCode = errorCode;
			Message = message;
		}
	}
}