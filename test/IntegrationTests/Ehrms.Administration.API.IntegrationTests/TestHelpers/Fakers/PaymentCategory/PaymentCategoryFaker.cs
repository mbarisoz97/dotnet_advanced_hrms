using Bogus;
using Ehrms.Administration.API.Database.Models;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Fakers.PaymentCategorty;

internal class PaymentCategoryFaker : Faker<PaymentCategory>
{
	public PaymentCategoryFaker()
	{
		RuleFor(x => x.Id, f => f.Random.Guid());
		RuleFor(x => x.Name, f => f.Name.Random.Words());
	}
}