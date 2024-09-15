using FluentValidation;
using Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

namespace Ehrms.Administration.API.Validation.PaymentCategory;

public class UpdatePaymentCategoryCommandValidator : AbstractValidator<UpdatePaymentCategoryCommand>
{
	public UpdatePaymentCategoryCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEqual(Guid.Empty);

		RuleFor(x => x.Name)
		   .NotEmpty();
	}
}