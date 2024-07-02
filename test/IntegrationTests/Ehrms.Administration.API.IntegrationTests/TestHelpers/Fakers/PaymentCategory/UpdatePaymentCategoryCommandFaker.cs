using Bogus;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Fakers.PaymentCategorty;

internal class UpdatePaymentCategoryCommandFaker : Faker<UpdatePaymentCategoryCommand>
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