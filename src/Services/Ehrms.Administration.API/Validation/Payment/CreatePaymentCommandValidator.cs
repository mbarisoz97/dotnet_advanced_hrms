using FluentValidation;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.API.Validation.Payment;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x=>x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.EmployeeId)
            .NotEqual(Guid.Empty);

		RuleFor(x => x.PaymentCategoryId)
            .NotEqual(Guid.Empty);
	}
}
