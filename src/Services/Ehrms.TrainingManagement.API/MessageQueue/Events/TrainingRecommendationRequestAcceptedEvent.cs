namespace Ehrms.TrainingManagement.API.MessageQueue.Events;

public sealed class TrainingRecommendationRequestAcceptedEvent
{
    public Guid RequestId { get; set; }
    public Guid ProjectId { get; set; }
}