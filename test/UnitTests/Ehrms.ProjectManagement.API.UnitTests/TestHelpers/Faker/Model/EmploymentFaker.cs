using Ehrms.ProjectManagement.API.Database.Models;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker;

internal class EmploymentFaker : Faker<Employment>
{
	public EmploymentFaker()
	{
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(e => e.StartedAt, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
	}

	public EmploymentFaker WithProject(Project project)
	{
		RuleFor(x => x.Project, project);
		return this;
	}

	public EmploymentFaker WithEmployee(Employee employee)
	{
		RuleFor(x => x.Employee, employee);
		return this;
	}

	public EmploymentFaker WithStartDate(DateOnly? startDate = null)
	{
		startDate ??= new DateOnly();

		RuleFor(x=> x.StartedAt, startDate);
		return this;
	}

	public EmploymentFaker WithEndDate(DateOnly? endDate = null)
	{
		endDate ??= new DateOnly();

		RuleFor(x => x.EndedAt, endDate);
		return this;
	}
}