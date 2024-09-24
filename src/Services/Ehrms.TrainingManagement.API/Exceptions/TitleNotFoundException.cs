namespace Ehrms.TrainingManagement.API.Exceptions;

internal sealed class TitleNotFoundException : CustomNotFoundException
{
    public TitleNotFoundException(string message) : base(message)
    {
    }
}