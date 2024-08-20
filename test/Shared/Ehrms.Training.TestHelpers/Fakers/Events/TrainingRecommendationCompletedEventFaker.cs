namespace Ehrms.Training.TestHelpers.Fakers.Events;

public sealed class TrainingRecommendationCompletedEventFaker : Faker<TrainingRecommendationCompletedEvent>
{
    public TrainingRecommendationCompletedEventFaker WithRequestId(Guid id)
    {
        RuleFor(x => x.RequestId, id);
        return this;
    }
}