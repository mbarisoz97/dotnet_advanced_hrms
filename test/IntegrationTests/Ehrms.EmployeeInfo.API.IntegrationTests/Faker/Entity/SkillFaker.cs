using Bogus;
using Ehrms.EmployeeInfo.API.Models;

namespace Ehrms.EmployeeInfo.API.IntegrationTests;

internal class SkillFaker : Faker<Skill>
{
    public SkillFaker()
    {
        RuleFor(e => e.Name, f => f.Name.Random.AlphaNumeric(6));
    }
}