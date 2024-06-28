using FluentValidation;

namespace Ehrms.ProjectManagement.API.Validation.Project;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
	public CreateProjectCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MinimumLength(Consts.MinProjectNameLength)
			.MaximumLength(Consts.MaxProjectNameLength);

		RuleFor(x => x.Description)
			.NotEmpty();
	}
}