using FluentValidation;

namespace Ehrms.EmployeeInfo.API.Validation;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
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
