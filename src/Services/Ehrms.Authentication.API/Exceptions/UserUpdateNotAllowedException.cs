namespace Ehrms.Authentication.API.Exceptions;

public class UserUpdateNotAllowedException : Exception
{
    public UserUpdateNotAllowedException(string message = "") : base(message)
    {
    }
}