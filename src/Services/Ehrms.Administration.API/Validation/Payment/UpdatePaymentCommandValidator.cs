using FluentValidation;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.API.Validation.Payment;

public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
{
	public UpdatePaymentCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEqual(Guid.Empty);

		RuleFor(x => x.Amount)
			.GreaterThan(0);
	}
}