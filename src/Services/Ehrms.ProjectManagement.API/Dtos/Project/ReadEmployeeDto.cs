namespace Ehrms.ProjectManagement.API.Dtos.Project;

public sealed class ReadEmployeeDto
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
}