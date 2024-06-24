namespace Ehrms.TrainingManagement.API.Models;

public sealed class Employee : BaseEntity
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public ICollection<Training> Trainings { get; set; } = [];
}