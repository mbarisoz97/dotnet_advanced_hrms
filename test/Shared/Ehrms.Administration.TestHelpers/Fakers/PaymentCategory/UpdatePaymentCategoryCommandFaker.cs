using Bogus;
using Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

namespace Ehrms.Administration.TestHelpers.Fakers.PaymentCategory;

public class UpdatePaymentCategoryCommandFaker : Faker<UpdatePaymentCategoryCommand>
{
    public UpdatePaymentCategoryCommandFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.Words());
    }

    public UpdatePaymentCategoryCommandFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }
}