namespace Ehrms.ProjectManagement.API.Dtos.Project;

public sealed class ProjectEmploymentDto
{
	public Guid Id { get; set; }
	public DateOnly StartedAt { get; set; }
	public DateOnly? EndedAt { get; set; }
	public Guid EmployeeId { get; set; }
	public string? EmployeeName { get; set; }
}