﻿namespace Ehrms.ProjectManagement.API.Dtos.Project;

public class UpdateProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Guid> EmployeeIdCollection { get; set; } = [];
}
