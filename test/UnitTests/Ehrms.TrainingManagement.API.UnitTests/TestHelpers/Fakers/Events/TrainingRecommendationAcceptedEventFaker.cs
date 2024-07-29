namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Models;

internal class TrainingRecommendationAcceptedEventFaker : Faker<TrainingRecommendationRequestAcceptedEvent>
{
    internal TrainingRecommendationAcceptedEventFaker WithRequestId(Guid id)
    {
        RuleFor(x=>x.RequestId, id);
        return this;
    }
}