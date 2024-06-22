namespace Ehrms.EmployeeInfo.API.Dtos.Employee;

public sealed class CreateEmployeeDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public ICollection<Guid> Skills { get; set; } = [];
}