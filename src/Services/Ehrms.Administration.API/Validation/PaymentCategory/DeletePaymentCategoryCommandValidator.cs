using FluentValidation;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.API.Validation.PaymentCategory;

public class DeletePaymentCategoryCommandValidator : AbstractValidator<DeletePaymentCategoryCommand>
{
	public DeletePaymentCategoryCommandValidator()
	{
		RuleFor(x => x.Id).NotEqual(Guid.Empty);
	}
}