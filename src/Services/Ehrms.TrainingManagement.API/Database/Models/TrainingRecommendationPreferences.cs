namespace Ehrms.TrainingManagement.API.Database.Models;

public sealed class TrainingRecommendationPreferences : BaseEntity
{
    public Project? Project { get; set; }
    public Title? Title { get; set; }
    public ICollection<Skill> Skills { get; set; } = [];
}