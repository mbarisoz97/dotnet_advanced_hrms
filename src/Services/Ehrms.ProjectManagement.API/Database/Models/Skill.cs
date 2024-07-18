namespace Ehrms.ProjectManagement.API.Database.Models;

public class Skill : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public ICollection<Project> Projects { get; set; } = [];
}