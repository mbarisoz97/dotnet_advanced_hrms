﻿using System.ComponentModel.DataAnnotations;

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

	[Required]
	public ICollection<Guid> RequiredSkills { get; set; } = [];
}
