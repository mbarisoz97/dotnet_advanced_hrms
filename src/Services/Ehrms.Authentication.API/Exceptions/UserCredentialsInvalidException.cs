namespace Ehrms.Authentication.API.Exceptions;

public class UserCredentialsInvalidException : Exception
{
    public UserCredentialsInvalidException(string message = "") : base(message)
    {
    }
}