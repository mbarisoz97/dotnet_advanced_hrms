namespace Ehrms.EmployeeInfo.API.Exceptions;

public class SkillNotFoundException : CustomNotFoundException
{
    public SkillNotFoundException(string message = "") : base(message)
    {
    }
}
