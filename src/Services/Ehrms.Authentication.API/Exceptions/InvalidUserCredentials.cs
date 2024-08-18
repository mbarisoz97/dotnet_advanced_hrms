namespace Ehrms.Authentication.API.Exceptions;

public class InvalidUserCredentials : Exception
{
    public InvalidUserCredentials(string message = "") : base(message)
    {
    }
}