namespace Ehrms.TrainingManagement.API.Database.Models;

public sealed class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Title? Title { get; set; }
    public ICollection<Skill> Skills { get; set; } = [];
    public ICollection<Training> Trainings { get; set; } = [];
    public ICollection<TrainingRecommendationResult> TrainingRecommendations { get; set; } = [];
}