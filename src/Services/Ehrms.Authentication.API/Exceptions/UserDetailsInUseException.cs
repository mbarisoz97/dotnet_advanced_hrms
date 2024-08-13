namespace Ehrms.Authentication.API.Exceptions;

public class UserDetailsInUseException : Exception
{
    public UserDetailsInUseException(string message = "") : base(message)
    {
    }
}