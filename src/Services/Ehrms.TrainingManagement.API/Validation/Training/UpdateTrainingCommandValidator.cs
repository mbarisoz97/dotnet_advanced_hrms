using FluentValidation;

namespace Ehrms.TrainingManagement.API.Validation.Training;

public class UpdateTrainingCommandValidator : AbstractValidator<UpdateTrainingCommand>
{
	public UpdateTrainingCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MinimumLength(Consts.MinTrainingNameLength)
			.MaximumLength(Consts.MaxTrainingNameLength);

		RuleFor(x => x.Description)
			 .NotEmpty();
	}
}