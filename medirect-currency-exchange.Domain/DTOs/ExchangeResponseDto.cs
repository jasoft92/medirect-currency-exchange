using System.Net;
using medirect_currency_exchange.Contracts;

namespace medirect_currency_exchange.Domain.DTOs
{
	public class ExchangeResponseDto
	{
		public double CustomerId { get; }

		public decimal SourceAccountBalance { get; }

		public string SourceCurrencyCode { get; }

		public decimal TargetAccountBalance { get; }

		public string TargetCurrencyCode { get; }

		public decimal ExchangeAmount { get; }

		public ExchangeResponseDto(double customerId, decimal sourceAccountBalance, string sourceCurrencyCode, decimal targetAccountBalance, string targetCurrencyCode, decimal exchangeAmount)
		{
			CustomerId = customerId;
			SourceAccountBalance = sourceAccountBalance;
			SourceCurrencyCode = sourceCurrencyCode;
			TargetAccountBalance = targetAccountBalance;
			TargetCurrencyCode = targetCurrencyCode;
			ExchangeAmount = exchangeAmount;
		}
	}
}
