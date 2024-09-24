using Ehrms.TrainingManagement.API.Handlers.Recommendation.Commands;

namespace Ehrms.Training.TestHelpers.Fakers.Commands;

public sealed class DeleteTrainingRecommendationPreferenceCommandFaker : Faker<DeleteTrainingRecommendationPreferenceCommand>
{
    public DeleteTrainingRecommendationPreferenceCommandFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
    }

    public DeleteTrainingRecommendationPreferenceCommandFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }
}