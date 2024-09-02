namespace Ehrms.EmployeeInfo.API.Database.Models;

public sealed class Title : BaseEntity
{
    public string TitleName { get; set; } = string.Empty;
    public ICollection<Employee> Employees { get; set; } = [];
}