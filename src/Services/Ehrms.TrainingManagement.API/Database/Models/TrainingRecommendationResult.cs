namespace Ehrms.TrainingManagement.API.Database.Models;

public sealed class TrainingRecommendationResult : BaseEntity
{
	public Skill? Skill { get; set; }
    public TrainingRecommendationRequest? RecommendationRequest { get; set; }
    public ICollection<Employee> Employees { get; set; } = [];
}