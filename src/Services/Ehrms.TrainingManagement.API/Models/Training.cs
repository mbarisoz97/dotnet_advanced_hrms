namespace Ehrms.TrainingManagement.API.Models;

public sealed class Training : BaseEntity
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public DateTime PlannedAt { get; set; }
	public  ICollection<Employee> Participants { get; set; } = [];
}