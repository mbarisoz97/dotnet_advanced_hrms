using Ehrms.ProjectManagement.API.Models;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker;

internal class EmploymentFaker : Faker<Employment>
{
    public EmploymentFaker()
    {
        RuleFor(e => e.StartedAt, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
    }

    public EmploymentFaker WithProject(Project project)
    {
        RuleFor(x=>x.Project, project);
        return this;
    }

    public EmploymentFaker WithEmployee(Employee employee)
    {
        RuleFor(x => x.Employee, employee);
        return this;
    }
}