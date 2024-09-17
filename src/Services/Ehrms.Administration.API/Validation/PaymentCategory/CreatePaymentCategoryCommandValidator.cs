using FluentValidation;
using Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

namespace Ehrms.Administration.API.Validation.PaymentCategory;

public class CreatePaymentCategoryCommandValidator : AbstractValidator<CreatePaymentCategoryCommand>
{
	public CreatePaymentCategoryCommandValidator()
	{
		RuleFor(x => x.Name)
		   .NotEmpty();
	}
}
