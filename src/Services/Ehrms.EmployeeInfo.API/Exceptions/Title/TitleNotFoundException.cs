namespace Ehrms.EmployeeInfo.API.Exceptions.Title;

public sealed class TitleNotFoundException : Exception
{
    public TitleNotFoundException(string message = "") : base(message)
    {
    }
}