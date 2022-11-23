using Newtonsoft.Json;

namespace medirect_currency_exchange.Application.Clients.Models
{
	public class RateApiError
	{
		[JsonProperty("error")]
		public ErrorDetails? ErrorDetails { get; set; }
	}

	public class ErrorDetails
	{
		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
