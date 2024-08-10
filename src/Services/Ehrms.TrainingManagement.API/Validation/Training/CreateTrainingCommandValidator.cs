using FluentValidation;

namespace Ehrms.TrainingManagement.API.Validation.Training;

public class CreateTrainingCommandValidator : AbstractValidator<CreateTrainingCommand>
{
    public CreateTrainingCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(Consts.MinTrainingNameLength)
            .MaximumLength(Consts.MaxTrainingNameLength);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}