namespace Ehrms.TrainingManagement.API.Database.Models;

public sealed class Title : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<TrainingRecommendationPreferences> TrainingRecommendationPreferences { get; set; } = [];
}