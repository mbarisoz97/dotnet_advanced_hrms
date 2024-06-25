namespace Ehrms.Shared.Exceptions;
public abstract class CustomNotFoundException : Exception
{
    protected CustomNotFoundException(string message) : base(message) { }
}