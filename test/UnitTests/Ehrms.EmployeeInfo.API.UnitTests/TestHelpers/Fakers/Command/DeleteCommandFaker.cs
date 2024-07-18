using Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

namespace Ehrms.EmployeeInfo.API.UnitTests.TestHelpers.Fakers.Command;

internal class DeleteCommandFaker : Faker<DeleteSkillCommand>
{
	public DeleteCommandFaker()
	{
		RuleFor(p => p.Id, f => f.Random.Guid());
	}

	public DeleteCommandFaker WithId(Guid id)
	{
		RuleFor(p => p.Id, id);
		return this;
	}
}