namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Models;

internal class ProjectFaker : Faker<Project>
{
	public ProjectFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(e => e.Name, f => f.Random.Word());
	}

	public ProjectFaker WithEmployees(ICollection<Employee> employees)
	{
		RuleFor(x => x.Employees, employees);
		return this;
	}

	public ProjectFaker WithRequiredSkills(ICollection<Skill> skills)
	{
		RuleFor(x => x.RequiredSkills, skills);
		return this;
	}
}