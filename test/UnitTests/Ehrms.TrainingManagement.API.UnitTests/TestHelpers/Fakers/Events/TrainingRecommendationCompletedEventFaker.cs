namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Models;

internal class TrainingRecommendationCompletedEventFaker : Faker<TrainingRecommendationCompletedEvent>
{
    internal TrainingRecommendationCompletedEventFaker WithRequestId(Guid id)
    {
        RuleFor(x => x.RequestId, id);
        return this;
    }
}