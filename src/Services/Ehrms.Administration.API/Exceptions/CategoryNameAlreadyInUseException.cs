using Ehrms.Shared.Exceptions;

namespace Ehrms.Administration.API.Exceptions;

public class CategoryNameAlreadyInUseException : CustomAlreadyInUseException
{
	public CategoryNameAlreadyInUseException(string message = "") : base(message)
	{
	}
}