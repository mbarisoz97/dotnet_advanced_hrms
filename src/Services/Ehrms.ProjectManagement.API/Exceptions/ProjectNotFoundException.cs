namespace Ehrms.ProjectManagement.API.Exceptions;

public class ProjectNotFoundException : CustomNotFoundException
{
    public ProjectNotFoundException(string message) : base(message)
    {
    }
}