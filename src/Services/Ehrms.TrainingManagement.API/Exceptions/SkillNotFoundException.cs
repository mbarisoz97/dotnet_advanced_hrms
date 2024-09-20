namespace Ehrms.TrainingManagement.API.Exceptions;

internal sealed class SkillNotFoundException : CustomNotFoundException
{
    public SkillNotFoundException(string message) : base(message)
    {
    }
}