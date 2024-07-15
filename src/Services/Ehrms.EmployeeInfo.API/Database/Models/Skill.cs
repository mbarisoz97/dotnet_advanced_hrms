namespace Ehrms.EmployeeInfo.API.Database.Models;

public sealed class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<Employee> Employees { get; set; } = [];
}