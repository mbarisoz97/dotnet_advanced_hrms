namespace Ehrms.EmployeeInfo.API.Database.Models;

public sealed class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public ICollection<Skill> Skills { get; set; } = [];
}