using Bogus;
using Ehrms.Administration.API.Models;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Fakers;

internal class PaymentCriteriaFaker : Faker<PaymentCriteria>
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

	public PaymentCriteriaFaker WithEmployee(Employee employee)
	{
		RuleFor(x => x.Employee, employee);
		return this;
	}

	public PaymentCriteriaFaker WithPaymentCategory(PaymentCategory paymentCategory)
	{
		RuleFor(x => x.PaymentCategory, paymentCategory);
		return this;
	}
}