namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Models;

internal class EmployeeFaker : Faker<Employee>
{
	public EmployeeFaker()
	{
		RuleFor(e => e.FirstName, f => f.Name.FirstName());
		RuleFor(e => e.LastName, f => f.Name.LastName());
	}
}
