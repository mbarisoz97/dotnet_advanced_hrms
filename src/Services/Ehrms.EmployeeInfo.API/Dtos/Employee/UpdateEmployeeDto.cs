namespace Ehrms.EmployeeInfo.API.Dtos.Employee;

public sealed class UpdateEmployeeDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public ICollection<Guid> Skills { get; set; } = [];
}