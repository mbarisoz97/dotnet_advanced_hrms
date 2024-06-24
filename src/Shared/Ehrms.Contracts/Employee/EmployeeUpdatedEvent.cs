namespace Ehrms.Contracts.Employee;

public sealed class EmployeeUpdatedEvent
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
}