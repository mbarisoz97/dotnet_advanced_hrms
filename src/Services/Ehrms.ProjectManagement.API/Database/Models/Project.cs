namespace Ehrms.ProjectManagement.API.Database.Models;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Employment> Employments { get; set; } = [];
}