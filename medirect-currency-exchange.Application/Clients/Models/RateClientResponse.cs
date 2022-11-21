using Newtonsoft.Json;

namespace medirect_currency_exchange.Application.Clients.Models
{
	internal class RateClientResponse
	{
		[JsonProperty("success")] public bool IsSuccess { get; set; }

		[JsonProperty("result")] public decimal Rate { get; set; }
	}
}