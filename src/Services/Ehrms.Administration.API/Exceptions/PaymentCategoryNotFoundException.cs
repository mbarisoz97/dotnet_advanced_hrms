using Ehrms.Shared.Exceptions;

namespace Ehrms.Administration.API.Exceptions;

public class PaymentCategoryNotFoundException : CustomNotFoundException
{
	public PaymentCategoryNotFoundException(string message) : base(message)
	{
	}
}

public class EmployeeNotFoundException : CustomNotFoundException
{
	public EmployeeNotFoundException(string message) : base(message)
	{
	}
}