using Bogus;
using Ehrms.Administration.API.Models;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Fakers;

internal class EmployeeFaker : Faker<Employee>
{
	public EmployeeFaker()
	{
		RuleFor(x => x.Id, f => f.Random.Guid());
		RuleFor(x => x.FirstName, f => f.Name.FirstName());
		RuleFor(x => x.LastName, f => f.Name.LastName());
	}
}

internal class PaymentCriteriaFaker : Faker<PaymentCriteria>
{
	public PaymentCriteriaFaker()
	{
		RuleFor(x => x.CreatedAt, f => f.Date.Past());
	}

	public PaymentCriteriaFaker WithEmployee(Employee employee)
	{
		RuleFor(x => x.Employee, employee);
		return this;
	}
}