namespace Ehrms.TrainingManagement.API.MessageQueue.Events;

public sealed class TrainingRecommendationCompletedEvent
{
    public Guid RequestId { get; set; }
}