namespace Ehrms.EmployeeInfo.API.Exceptions;

public class SkillNameIsInUseException : CustomAlreadyInUseException
{
	public SkillNameIsInUseException(string message = "") : base(message)
	{
	}
}
