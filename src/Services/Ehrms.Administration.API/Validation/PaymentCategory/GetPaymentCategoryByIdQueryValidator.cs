using FluentValidation;
using Ehrms.Administration.API.Handlers.PaymentCategory.Queries;

namespace Ehrms.Administration.API.Validation.PaymentCategory;

public class GetPaymentCategoryByIdQueryValidator : AbstractValidator<GetPaymentCategoryQuery>
{
	public GetPaymentCategoryByIdQueryValidator()
	{
		RuleFor(x => x.Id)
			.NotEqual(Guid.Empty);
	}
}