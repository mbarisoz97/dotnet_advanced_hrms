using Ehrms.Contracts.Project;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Events;

internal class ProjectCreatedEventFaker : Faker<ProjectCreatedEvent>
{
	public ProjectCreatedEventFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(e => e.Name, f => f.Random.Word());
	}

	public ProjectCreatedEventFaker WithEmployees(ICollection<Employee> employees)
	{
		RuleFor(e => e.Employees, f => employees.Select(x => x.Id).ToList());
		return this;
	}

	public ProjectCreatedEventFaker WithRequiredSkills(ICollection<Skill> skills)
	{
		RuleFor(e => e.RequiredSkills, f => skills.Select(x => x.Id).ToList());
		return this;
	}
}