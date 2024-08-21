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
		
		RuleFor(x => x.StartsAt)
			.NotNull();

		RuleFor(x => x.EndsAt)
			.NotNull();
        
		RuleFor(x => x.EndsAt)
			.GreaterThan(x => x.StartsAt);
	}
}