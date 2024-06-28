using FluentValidation;

namespace Ehrms.ProjectManagement.API.Validation.Project;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
	public UpdateProjectCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEqual(Guid.Empty);

		RuleFor(x => x.Name)
			.NotEmpty()
			.MinimumLength(Consts.MinProjectNameLength)
			.MaximumLength(Consts.MaxProjectNameLength);

		RuleFor(x => x.Description)
			.NotEmpty();
	}
}