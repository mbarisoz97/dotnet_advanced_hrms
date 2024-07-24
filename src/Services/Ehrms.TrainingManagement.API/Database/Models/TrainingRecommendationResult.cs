namespace Ehrms.TrainingManagement.API.Database.Models;

public sealed class TrainingRecommendationResult : BaseEntity
{
	public TrainingRecommendationRequest? RecommendationRequest { get; set; }
	public Skill? Skill { get; set; }
	public ICollection<Employee> Employees { get; set; } = [];
}