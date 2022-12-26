using FluentValidation;

namespace Quotes.API.Validators;

public class QuoteValidator: AbstractValidator<QuoteModel>
{
	public QuoteValidator()
	{
        RuleFor(quote => quote.Message).NotNull().NotEmpty().Length(3, 150);
    }
}
