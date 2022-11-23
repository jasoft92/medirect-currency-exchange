using FluentValidation;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Domain;

namespace medirect_currency_exchange.Validators
{
	public class ExchangeRequestValidator : AbstractValidator<CurrencyExchangeRequest>
	{
		public ExchangeRequestValidator()
		{
			RuleFor(r => r.CustomerId)
				.NotEmpty().WithMessage(ValidationErrorMessages.CustomerIdCannotBeEmpty)
				.GreaterThan(0).WithMessage(ValidationErrorMessages.CustomerIdMustBeGreaterThanZero);

			RuleFor(r => r.SourceCurrency)
				.NotEmpty().WithMessage(ValidationErrorMessages.SourceCurrencyCannotBeEmpty)
				.Length(3).WithMessage(ValidationErrorMessages.IncorrectSourceCurrencyFormat);


			RuleFor(r => r.TargetCurrency)
				.NotEmpty().WithMessage(ValidationErrorMessages.TargetCurrencyCannotBeEmpty)
				.Length(3).WithMessage(ValidationErrorMessages.IncorrectTargetCurrencyFormat);

			RuleFor(r => r.ExchangeAmount)
				.NotEmpty().WithMessage(ValidationErrorMessages.ExchangeAmountCannotBeEmpty)
				.GreaterThan(0).WithMessage(ValidationErrorMessages.ExchangeAmountMustBeGreaterThanZero);
		}
	}
}
