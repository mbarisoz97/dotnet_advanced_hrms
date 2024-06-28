using Ehrms.EmployeeInfo.API.Handlers.Employee.Command;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Fakers.Dto;

internal class UpdateEmployeeCommandFaker : Faker<UpdateEmployeeCommand>
{
	public UpdateEmployeeCommandFaker()
	{
		RuleFor(e => e.Id, Guid.NewGuid());
		RuleFor(e => e.FirstName, f => f.Name.FirstName());
		RuleFor(e => e.LastName, f => f.Name.LastName());
		RuleFor(e => e.DateOfBirth, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
		RuleFor(e => e.Qualification, f => f.Name.JobTitle());
	}

	public UpdateEmployeeCommandFaker WithId(Guid id)
	{
		RuleFor(e => e.Id, id);
		return this;
	}
}