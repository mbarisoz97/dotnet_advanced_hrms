namespace Ehrms.Web.Models;

public sealed class ReadTrainingRecommendationRequestModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public RequestStatus RequestStatus { get; set; }
}

public enum RequestStatus
{
    Accepted,
    Pending,
    Completed,
    Cancelled,
}