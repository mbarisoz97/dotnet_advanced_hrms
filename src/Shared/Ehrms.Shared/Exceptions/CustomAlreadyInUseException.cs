namespace Ehrms.Shared.Exceptions;

public abstract class CustomAlreadyInUseException : Exception
{
	protected CustomAlreadyInUseException(string message = "") : base(message)
	{
	}
}