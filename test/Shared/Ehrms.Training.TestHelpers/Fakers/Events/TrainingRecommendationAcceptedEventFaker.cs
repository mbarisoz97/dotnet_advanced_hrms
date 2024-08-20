namespace Ehrms.Training.TestHelpers.Fakers.Events;

public sealed class TrainingRecommendationAcceptedEventFaker : Faker<TrainingRecommendationRequestAcceptedEvent>
{
    public TrainingRecommendationAcceptedEventFaker WithRequestId(Guid id)
    {
        RuleFor(x=>x.RequestId, id);
        return this;
    }
}