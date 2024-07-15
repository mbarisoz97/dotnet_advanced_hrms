namespace Ehrms.ProjectManagement.API.Database.Models;

public class Employment : BaseEntity
{
    public DateOnly StartedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateOnly? EndedAt { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }
    public virtual Project? Project { get; set; }
    public virtual Employee? Employee { get; set; }
}