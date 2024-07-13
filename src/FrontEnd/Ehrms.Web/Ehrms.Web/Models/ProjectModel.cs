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
}



public sealed class CreateProjectModel
{
	[Required]
	[Length(2, 100)]
	public string Name { get; set; } = string.Empty;

	[Required]
	public string Description { get; set; } = string.Empty;

	[Required]
	public ICollection<Guid> SelectedProjects { get; set; } = [];
}