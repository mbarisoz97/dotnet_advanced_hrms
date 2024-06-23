using Bogus;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Command;

internal class UpdateProjectCommandFaker : Faker<UpdateProjectCommand>
{
    public UpdateProjectCommandFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10));
        RuleFor(x => x.Description, f => f.Name.Random.AlphaNumeric(10));
    }
}