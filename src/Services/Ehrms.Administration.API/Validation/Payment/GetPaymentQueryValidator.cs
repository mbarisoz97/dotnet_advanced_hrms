using FluentValidation;
using Ehrms.Administration.API.Handlers.Payment.Queries;

namespace Ehrms.Administration.API.Validation.Payment;

public class GetPaymentQueryValidator : AbstractValidator<GetPaymentQuery>
{
	public GetPaymentQueryValidator()
	{
		RuleFor(x => x.Id)
			.NotEqual(Guid.Empty);
	}
}