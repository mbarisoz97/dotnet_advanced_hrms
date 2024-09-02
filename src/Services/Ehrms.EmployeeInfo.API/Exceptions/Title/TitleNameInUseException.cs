namespace Ehrms.EmployeeInfo.API.Exceptions.Title;

public sealed class TitleNameInUseException : Exception
{
    public TitleNameInUseException(string message = "") : base(message)
    {
    }
}