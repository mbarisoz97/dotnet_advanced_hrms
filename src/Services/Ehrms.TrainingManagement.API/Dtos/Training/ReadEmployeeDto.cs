namespace Ehrms.TrainingManagement.API.Dtos.Training;

public sealed class ReadEmployeeDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}