using Bogus;
using Ehrms.ProjectManagement.API.Models;
using Ehrms.ProjectManagement.API.Handlers.Project.Commands;

namespace Ehrms.ProjectManagement.API.IntegrationTests.TestHelpers.Faker;

internal class UpdateProjectCommandFaker : Faker<UpdateProjectCommand>
{
	public UpdateProjectCommandFaker()
	{
		RuleFor(x => x.Id, Guid.NewGuid());
		RuleFor(x => x.Name, f => f.Name.Random.Words(2));
		RuleFor(x => x.Description, f => f.Random.Words(2));
	}

	public UpdateProjectCommandFaker WithId(Guid id)
	{
		RuleFor(x => x.Id, id);
		return this;
	}

	public UpdateProjectCommandFaker WithProject(Project project)
	{
		RuleFor(x => x.Id, project.Id);
		return this;
	}

	public UpdateProjectCommandFaker WithEmployees(ICollection<Employee> employees)
	{
		var employeeIdCollection = employees.Select(x => x.Id).ToList() ?? [];
		RuleFor(x => x.Employees, employeeIdCollection);
		return this;
	}
}