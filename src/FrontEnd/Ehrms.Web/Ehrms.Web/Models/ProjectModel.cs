using System.ComponentModel.DataAnnotations;

namespace Ehrms.Web.Models;

public class ProjectModel
{
	public Guid Id { get; set; }

	[Required]
	[Length(2, 100)]
	public string Name { get; set; } = string.Empty;

	[Required]
	public string Description { get; set; } = string.Empty;

	[Required]
	public ICollection<Guid> Employees { get; set; } = [];
}

public sealed class EmploymentModel
{
	public Guid Id { get; set; }
	public DateOnly StartedAt { get; set; }
	public DateOnly? EndedAt { get; set; }
	public Guid EmployeeId { get; set; }
	public string EmployeeName { get; set; } = string.Empty;
}

public sealed class GroupedEmploymentModel
{
	public Guid EmployeeId { get; set; }
	public string EmployeeName { get; set; } = string.Empty;
	public ICollection<EmploymentModel> EmploymentRecords { get; set; } = [];
}