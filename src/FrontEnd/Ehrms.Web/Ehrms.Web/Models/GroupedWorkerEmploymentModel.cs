namespace Ehrms.Web.Models;

public sealed class GroupedWorkerEmploymentModel
{
	public Guid ProjectId { get; set; }
	public string ProjectName { get; set; } = string.Empty;
	public ICollection<WorkerEmploymentModel> EmploymentRecords { get; set; } = [];
}