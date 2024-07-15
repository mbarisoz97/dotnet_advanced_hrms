using Bogus;
using Ehrms.EmployeeInfo.API.Database.Models;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Fakers.Entity;

internal class SkillFaker : Faker<Skill>
{
    public SkillFaker()
    {
        RuleFor(e => e.Name, f => f.Name.Random.AlphaNumeric(6));
    }
}