namespace Ehrms.Web.Models.Training;

public class CreateTrainingPreferenceModel
{
    public Guid ProjectId { get; set; }
    public Guid TitleId { get; set; }
    public IEnumerable<Guid> Skills { get; set; } = [];
}

public class TrainingPreferenceModel
{
    public Guid Id { get; set; }
    public TitleSummary? Title { get; set; }
    public ProjectSummary? Project { get; set; }
    public IEnumerable<SkillSummary> Skills { get; set; } = [];
}

public class ProjectSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TitleSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SkillSummary
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}