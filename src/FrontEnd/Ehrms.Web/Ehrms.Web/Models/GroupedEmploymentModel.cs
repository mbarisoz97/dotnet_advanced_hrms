namespace Ehrms.Web.Models;

public sealed class GroupedProjectEmploymentModel
{
	public Guid EmployeeId { get; set; }
	public string EmployeeName { get; set; } = string.Empty;
	public ICollection<ProjectEmploymentModel> EmploymentRecords { get; set; } = [];
}