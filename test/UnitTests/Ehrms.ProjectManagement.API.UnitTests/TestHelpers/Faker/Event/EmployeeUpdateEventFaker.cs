namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Event;

internal class EmployeeUpdateEventFaker : Faker<EmployeeUpdatedEvent>
{
	public EmployeeUpdateEventFaker()
	{
		RuleFor(x => x.FirstName, f => f.Name.FirstName());
		RuleFor(x => x.LastName, f => f.Name.LastName());
	}
}