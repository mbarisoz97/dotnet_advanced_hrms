namespace Ehrms.Authentication.API.Exceptions;

public class UserPasswordResetFailedException : Exception
{
    public UserPasswordResetFailedException(string message = "") : base(message)
    {
    }
}