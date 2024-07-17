namespace Ehrms.ProjectManagement.API.Dtos.Project;

public sealed class WorkerEmploymentDto
{
	public Guid Id { get; set; }
	public DateOnly StartedAt { get; set; }
	public DateOnly? EndedAt { get; set; }
	public Guid ProjectId { get; set; }
	public string? ProjectName { get; set; }
}