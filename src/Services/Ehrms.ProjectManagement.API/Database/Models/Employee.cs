namespace Ehrms.ProjectManagement.API.Database.Models;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public virtual ICollection<Employment> Employments { get; set; } = [];
}