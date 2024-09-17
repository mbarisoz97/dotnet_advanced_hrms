using Bogus;
using Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

namespace Ehrms.Administration.TestHelpers.Fakers.PaymentCategory;

public class CreatePaymentCategoryCommandFaker : Faker<CreatePaymentCategoryCommand>
{
    public CreatePaymentCategoryCommandFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.Words());
    }

    public CreatePaymentCategoryCommandFaker WithName(string name)
    {
        RuleFor(x => x.Name, name);
        return this;
    }
}