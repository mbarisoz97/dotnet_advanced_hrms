namespace Ehrms.Authentication.API.Exceptions;

public class UserAccountInactiveException : Exception
{
    public UserAccountInactiveException(string message = "") : base(message)
    {
    }
}