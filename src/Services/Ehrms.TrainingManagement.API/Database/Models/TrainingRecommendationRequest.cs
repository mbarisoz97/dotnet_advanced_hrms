namespace Ehrms.TrainingManagement.API.Database.Models;

public enum RequestStatus
{
	Pending,
	Completed,
	Cancelled
}

public sealed class TrainingRecommendationRequest : BaseEntity
{
	public string Title { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
	public RequestStatus RequestStatus { get; set; }
	public Project? Project { get; set; }
	public ICollection<TrainingRecommendationResult> TrainingRecommendation { get; set; } = [];
}