namespace Ehrms.TrainingManagement.API.Dtos.Training;

public sealed class CreateTrainingDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 
    public DateTime PlannedAt { get; set; }
    public ICollection<Guid> Participants { get; set; } = [];
}