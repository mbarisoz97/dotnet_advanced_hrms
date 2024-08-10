using Ehrms.Contracts.Project;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Events;

internal class ProjectUpdatedEventFaker : Faker<ProjectUpdatedEvent>
{
	public ProjectUpdatedEventFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(e => e.Name, f => f.Random.Word());
	}

	public ProjectUpdatedEventFaker WithId(Guid id)
	{
		RuleFor(e => e.Id, id);
		return this;
	}

	public ProjectUpdatedEventFaker WithEmployees(ICollection<Employee> employees)
	{
		RuleFor(e => e.Employees, f => employees.Select(x => x.Id).ToList());
		return this;
	}

	public ProjectUpdatedEventFaker WithRequiredSkills(ICollection<Skill> skills)
	{
		RuleFor(e => e.RequiredSkills, f => skills.Select(x => x.Id).ToList());
		return this;
	}
}