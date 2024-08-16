namespace Ehrms.Authentication.API.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(string message = "") : base(message)
    {
    }
}