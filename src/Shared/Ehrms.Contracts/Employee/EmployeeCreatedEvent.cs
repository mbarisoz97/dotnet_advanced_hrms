namespace Ehrms.Contracts.Employee;

public sealed class EmployeeCreatedEvent
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<Guid> Skills { get; set; } = [];
}