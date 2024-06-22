namespace Ehrms.EmployeeInfo.API.Exceptions;

public class EmployeeNotFoundException : CustomNotFoundException
{
    public EmployeeNotFoundException(string message = "") : base(message)
    {
    }
}