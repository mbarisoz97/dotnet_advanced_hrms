using Bogus;

namespace Ehrms.ProjectManagement.API.IntegrationTests.TestHelpers.Faker;

internal class CreateProjectDtoFaker : Faker<CreateProjectDto>
{
    public CreateProjectDtoFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10));
        RuleFor(x => x.Description, f => f.Name.Random.AlphaNumeric(10));
    }
}
