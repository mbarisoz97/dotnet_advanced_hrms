namespace Ehrms.TrainingManagement.API.Dtos.Training;

public sealed class ReadTrainingRecommendationResultDto
{
    public string Title { get; set; } = string.Empty;
    public string Skill { get; set; } = string.Empty;
    public ICollection<ReadEmployeeDto>? Employees { get; set; }
}