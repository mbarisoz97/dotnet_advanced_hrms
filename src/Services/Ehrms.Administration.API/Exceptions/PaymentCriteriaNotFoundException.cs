using Ehrms.Shared.Exceptions;

namespace Ehrms.Administration.API.Exceptions;

public class PaymentCriteriaNotFoundException : CustomNotFoundException
{
    public PaymentCriteriaNotFoundException(string message) : base(message)
    {
    }
}