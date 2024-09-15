using Bogus;

namespace Ehrms.Administration.TestHelpers.Fakers.PaymentCategory;

public class PaymentCategoryFaker : Faker<API.Database.Models.PaymentCategory>
{
    public PaymentCategoryFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Name.Random.Words());
    }
}