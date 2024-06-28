using FluentValidation;

namespace Ehrms.EmployeeInfo.API.Validation;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
	public UpdateEmployeeCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEqual(Guid.Empty);

		RuleFor(x => x.Qualification)
			.NotEmpty();

		RuleFor(x => x.FirstName)
			.NotEmpty()
			.MinimumLength(Consts.MinNameLength)
			.MaximumLength(Consts.MaxNameLength);

		RuleFor(x => x.LastName)
			.NotEmpty()
			.MinimumLength(Consts.MinNameLength)
			.MaximumLength(Consts.MaxNameLength);
	}
}
