using Bogus;
using Ehrms.ProjectManagement.API.Models;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker;

internal class EmploymentFaker : Faker<Employment>
{
    public EmploymentFaker()
    {
        RuleFor(e => e.StartedAt, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
    }
}
