using Bogus;

namespace Ehrms.Administration.TestHelpers.Fakers;

public class PaymentCriteriaFaker : Faker<PaymentCriteria>
{
    public PaymentCriteriaFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.CreatedAt, f => f.Date.Past());
    }

    public PaymentCriteriaFaker WithPaymentId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }

    public PaymentCriteriaFaker WithEmployee(API.Database.Models.Employee employee)
    {
        RuleFor(x => x.Employee, employee);
        return this;
    }

    public PaymentCriteriaFaker WithPaymentCategory(API.Database.Models.PaymentCategory paymentCategory)
    {
        RuleFor(x => x.PaymentCategory, paymentCategory);
        return this;
    }
}