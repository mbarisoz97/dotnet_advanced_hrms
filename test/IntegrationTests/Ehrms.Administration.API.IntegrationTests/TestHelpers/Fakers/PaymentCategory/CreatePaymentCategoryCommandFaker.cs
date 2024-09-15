using Bogus;
using Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Fakers.PaymentCategorty;

internal class CreatePaymentCategoryCommandFaker : Faker<CreatePaymentCategoryCommand>
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