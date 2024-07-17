namespace Ehrms.EmployeeInfo.API.UnitTests.TestHelpers.Fakers.Command;

internal class UpdateEmployeeCommandFaker : Faker<UpdateEmployeeCommand>
{
	public UpdateEmployeeCommandFaker()
	{
		RuleFor(p => p.FirstName, f => f.Name.FirstName());
		RuleFor(p => p.LastName, f => f.Name.LastName());
		RuleFor(e => e.DateOfBirth, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
		RuleFor(e => e.Qualification, f => f.Name.JobTitle());
	}

	public UpdateEmployeeCommandFaker WithId(Guid id)
	{
		RuleFor(p => p.Id, id);
		return this;
	}

	public UpdateEmployeeCommandFaker WithSkills(ICollection<Guid> skills)
	{
		RuleFor(x => x.Skills, skills);
		return this;
	}
}