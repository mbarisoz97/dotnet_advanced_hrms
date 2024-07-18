namespace Ehrms.Web.Models;

public sealed class ProjectEmploymentModel
{
	public Guid Id { get; set; }
	public DateOnly StartedAt { get; set; }
	public DateOnly? EndedAt { get; set; }
	public Guid EmployeeId { get; set; }
	public string EmployeeName { get; set; } = string.Empty;
}
