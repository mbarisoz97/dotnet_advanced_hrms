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