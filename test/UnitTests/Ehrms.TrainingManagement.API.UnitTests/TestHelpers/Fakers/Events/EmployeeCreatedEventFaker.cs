using Ehrms.Contracts.Employee;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Events;

internal class EmployeeCreatedEventFaker : Faker<EmployeeCreatedEvent>
{
	public EmployeeCreatedEventFaker()
	{
		RuleFor(e => e.Id, Guid.NewGuid());
		RuleFor(e => e.FirstName, f => f.Name.FirstName());
		RuleFor(e => e.LastName, f => f.Name.LastName());
	}

	public EmployeeCreatedEventFaker WithSkills(ICollection<Skill> skills)
	{
		var skillIdCollection = skills.Select(x => x.Id).ToList();
		RuleFor(x => x.Skills, skillIdCollection);
		return this;	
	}
}