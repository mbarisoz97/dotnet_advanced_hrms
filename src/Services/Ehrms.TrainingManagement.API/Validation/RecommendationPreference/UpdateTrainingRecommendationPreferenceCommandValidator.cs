using FluentValidation;
using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.TrainingManagement.API.Validation.RecommendationPreference;

public class UpdateTrainingRecommendationPreferenceCommandValidator : AbstractValidator<UpdateTrainingRecommendationPreferenceCommand>
{
    public UpdateTrainingRecommendationPreferenceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.Skills)
            .NotEmpty();
    }
}
