namespace Ehrms.Contracts.Project;

public class ProjectCreatedEvent
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public ICollection<Guid> Employees { get; set; } = [];
	public ICollection<Guid> RequiredSkills { get; set; } = [];
}