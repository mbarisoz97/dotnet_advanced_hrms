using Ehrms.ProjectManagement.API.Database.Models;

namespace Ehrms.ProjectManagement.API.TestHelpers.Faker.Faker.Command;

internal class CreateProjectCommandFaker : Faker<CreateProjectCommand>
{
	public CreateProjectCommandFaker()
	{
		RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10));
		RuleFor(x => x.Description, f => f.Name.Random.AlphaNumeric(10));
	}

	public CreateProjectCommandFaker WithEmployees(params Employee[] employees)
	{
		var employeeIdCollection = employees.Select(x => x.Id).ToList();
		RuleFor(x => x.Employees, employeeIdCollection);
		return this;
	}

	public CreateProjectCommandFaker WithEmployees(ICollection<Employee> employees)
	{
		var employeeIdCollection = employees.Select(x => x.Id).ToList();
		RuleFor(x => x.Employees, employeeIdCollection);
		return this;
	}
}
