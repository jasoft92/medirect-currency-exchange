using System.Net;

namespace medirect_currency_exchange.Application.Exception
{
	public class ApiException : System.Exception
	{
		public HttpStatusCode HttpStatusCode { get; }

		public ApiException(HttpStatusCode httpStatusCode, string message) : base(message)
		{
			HttpStatusCode = httpStatusCode;
		}
	}
}
