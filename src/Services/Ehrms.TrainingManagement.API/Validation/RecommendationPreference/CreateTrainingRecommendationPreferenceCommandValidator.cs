using FluentValidation;
using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.TrainingManagement.API.Validation.RecommendationPreference;

public class CreateTrainingRecommendationPreferenceCommandValidator : AbstractValidator<CreateTrainingRecommendationPreferenceCommand>
{
    public CreateTrainingRecommendationPreferenceCommandValidator()
    {
        RuleFor(x => x.TitleId)
            .NotEqual(Guid.Empty);
        
        RuleFor(x => x.ProjectId)
            .NotEqual(Guid.Empty);
        
        RuleFor(x => x.Skills)
            .NotEmpty();
    }
}
