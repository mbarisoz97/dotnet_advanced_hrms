namespace Ehrms.TrainingManagement.API.Database.Models;

public sealed class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Employee> Employees { get; set; } = [];
	public ICollection<Project> Projects { get; set; } = [];
    public ICollection<TrainingRecommendationPreferences> TrainingRecommendationPreferences { get; set; } = [];
}