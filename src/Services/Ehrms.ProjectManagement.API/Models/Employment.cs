namespace Ehrms.ProjectManagement.API.Models;

public class Employment : BaseEntity
{
    public DateOnly StartedAt { get; set; } 
    public DateOnly? EndedAt { get; set; }
    public virtual Project? Project { get; set; }
    public virtual Employee? Employee { get; set; }
}