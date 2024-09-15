using Bogus;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.TestHelpers.Fakers.Payment;

public class CreatePaymentCommandFaker : Faker<CreatePaymentCommand>
{
    public CreatePaymentCommandFaker()
    {
        RuleFor(x => x.Amount, f => f.Random.Number(min: 1));
        RuleFor(x => x.EmployeeId, f => f.Random.Guid());
        RuleFor(x => x.PaymentCategoryId, f => f.Random.Guid());
    }

    public CreatePaymentCommandFaker WithEmployeeId(Guid id)
    {
        RuleFor(x => x.EmployeeId, id);
        return this;
    }

    public CreatePaymentCommandFaker WithPaymentCategoryId(Guid id)
    {
        RuleFor(x => x.PaymentCategoryId, id);
        return this;
    }

    public CreatePaymentCommandFaker WithAmount(decimal amount)
    {
        RuleFor(x => x.Amount, amount);
        return this;
    }
}