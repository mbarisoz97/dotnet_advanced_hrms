using Bogus;
using Ehrms.Administration.API.Handlers.Payment.Commands;

namespace Ehrms.Administration.TestHelpers.Fakers.Payment;

public class UpdatePaymentCommandFaker : Faker<UpdatePaymentCommand>
{
    public UpdatePaymentCommandFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Amount, f => f.Random.Number(min: 1));
    }

    public UpdatePaymentCommandFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }

    public UpdatePaymentCommandFaker WithAmount(decimal amount)
    {
        RuleFor(x => x.Amount, amount);
        return this;
    }
}