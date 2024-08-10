namespace Ehrms.TrainingManagement.API.MessageQueue.Events;

public sealed class TrainingRecommendationCancelledEvent
{
    public Guid RequestId { get; set; }
}