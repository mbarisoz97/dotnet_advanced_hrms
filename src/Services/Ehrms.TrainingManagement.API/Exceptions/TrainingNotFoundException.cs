namespace Ehrms.TrainingManagement.API.Exceptions;

internal sealed class TrainingNotFoundException : CustomNotFoundException
{
    public TrainingNotFoundException(string message) : base(message)
    {
    }
}