using Ehrms.EmployeeInfo.API.Handlers.Skill.Command;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Fakers.Dto;

internal class CreateSkillCommandFaker : Faker<CreateSkillCommand>
{
    public CreateSkillCommandFaker()
    {
        RuleFor(e => e.Name, f => f.Name.Random.AlphaNumeric(6));
    }
}
