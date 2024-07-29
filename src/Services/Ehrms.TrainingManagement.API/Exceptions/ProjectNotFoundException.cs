namespace Ehrms.TrainingManagement.API.Exceptions;

internal sealed class ProjectNotFoundException : CustomNotFoundException
{
    public ProjectNotFoundException(string message) : base(message)
    {
    }
}