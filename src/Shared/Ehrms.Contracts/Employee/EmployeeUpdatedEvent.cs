namespace Ehrms.Contracts.Employee;

public sealed class EmployeeUpdatedEvent
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public Guid TitleId { get; set; }
	public ICollection<Guid> Skills { get; set; } = [];
}