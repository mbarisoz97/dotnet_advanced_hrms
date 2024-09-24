namespace Ehrms.TrainingManagement.API.Dtos.RecommendationPreference;

public class ReadRecommendationPreferenceDto
{
    public Guid Id { get; set; }    
    public TitleDto? Title { get; set; }
    public ProjectDto? Project { get; set; }
    public ICollection<SkillDto> Skills { get; set; } = [];
}

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TitleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
