namespace Ehrms.EmployeeInfo.API.Exceptions.Title;

public class TitleNameInUseException : Exception
{
    public TitleNameInUseException(string message = "") : base(message)
    {
    }
}