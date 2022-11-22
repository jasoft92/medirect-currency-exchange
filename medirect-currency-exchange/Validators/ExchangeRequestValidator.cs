using FluentValidation;
using medirect_currency_exchange.Contracts;

namespace medirect_currency_exchange.Validators
{
	public class ExchangeRequestValidator : AbstractValidator<CurrencyExchangeRequest>
	{
		public ExchangeRequestValidator()
		{
			RuleFor(r => r.CustomerId)
				.NotEmpty().WithMessage("CustomerID cannot be empty")
				.GreaterThan(0).WithMessage("CustomerID must be greater than 0");

			RuleFor(r => r.SourceCurrency)
				.NotEmpty().WithMessage("Source currency code cannot be empty")
				.Length(3).WithMessage("Incorrect source currency format, currency code must be 3 characters long");


			RuleFor(r => r.TargetCurrency)
				.NotEmpty().WithMessage("Target currency code cannot be empty")
				.Length(3).WithMessage("Incorrect target currency format, currency code must be 3 characters long");

			RuleFor(r=>r.ExchangeAmount)
				.NotEmpty().WithMessage("Exchange amount cannot be left empty")
				.GreaterThan(0).WithMessage("Exchange amount must be greater than 0");
		}
	}
}
