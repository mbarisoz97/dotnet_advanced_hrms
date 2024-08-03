namespace Ehrms.TrainingManagement.API.Dtos.Training;

public sealed class ReadTrainingRequestDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public RequestStatus RequestStatus { get; set; }
}