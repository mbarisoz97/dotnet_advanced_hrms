using FluentValidation;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.API.Validation.Payment;

public class DeletePaymentCommandValidator : AbstractValidator<DeletePaymentCommand>
{
	public DeletePaymentCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEqual(Guid.Empty);
	}
}