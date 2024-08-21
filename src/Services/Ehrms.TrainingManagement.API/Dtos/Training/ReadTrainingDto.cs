namespace Ehrms.TrainingManagement.API.Dtos.Training;

public sealed class ReadTrainingDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime PlannedAt { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public ICollection<Guid> Participants { get; set; } = [];
}