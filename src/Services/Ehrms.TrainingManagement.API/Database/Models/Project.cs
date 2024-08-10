namespace Ehrms.TrainingManagement.API.Database.Models;

public sealed class Project : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public ICollection<Employee> Employees { get; set; } = [];
	public ICollection<Skill> RequiredSkills { get; set; } = [];
}
