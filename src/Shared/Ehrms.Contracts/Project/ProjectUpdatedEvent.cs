namespace Ehrms.Contracts.Project;

public class ProjectUpdatedEvent
{
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public ICollection<Guid> Employees { get; set; } = [];
}
