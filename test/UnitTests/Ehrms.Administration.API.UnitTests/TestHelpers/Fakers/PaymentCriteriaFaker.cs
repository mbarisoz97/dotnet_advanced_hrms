using Bogus;
using Ehrms.Administration.API.Models;

namespace Ehrms.Administration.API.UnitTests.TestHelpers.Fakers;

internal class PaymentCriteriaFaker : Faker<PaymentCriteria>
{
    public PaymentCriteriaFaker()
    {
        RuleFor(x=>x.Id, Guid.NewGuid());
        RuleFor(x => x.CreatedAt, f => f.Date.Past());
    }

    public PaymentCriteriaFaker WithExpirationDate()
    {
        RuleFor(x => x.CreatedAt, f => f.Date.Past());
        return this;
    }

    public PaymentCriteriaFaker WithEmployee(Employee employee)
    {
        RuleFor(x=>x.Employee, employee);
        return this;
    }
}
