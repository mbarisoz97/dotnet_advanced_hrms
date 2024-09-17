using FluentValidation;
using Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

namespace Ehrms.Administration.API.Validation.PaymentCategory;

public class DeletePaymentCategoryCommandValidator : AbstractValidator<DeletePaymentCategoryCommand>
{
	public DeletePaymentCategoryCommandValidator()
	{
		RuleFor(x => x.Id).NotEqual(Guid.Empty);
	}
}