using FluentValidation;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.API.Validation.PaymentCategory;

public class CreatePaymentCategoryCommandValidator : AbstractValidator<CreatePaymentCategoryCommand>
{
	public CreatePaymentCategoryCommandValidator()
	{
		RuleFor(x => x.Name)
		   .NotEmpty();
	}
}
