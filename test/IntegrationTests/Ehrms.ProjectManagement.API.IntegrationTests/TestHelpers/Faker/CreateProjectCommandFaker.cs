using Bogus;
using Ehrms.ProjectManagement.API.Handlers.Project.Commands;

namespace Ehrms.ProjectManagement.API.IntegrationTests.TestHelpers.Faker;

internal class CreateProjectCommandFaker : Faker<CreateProjectCommand>
{
    public CreateProjectCommandFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.Words(2));
        RuleFor(x => x.Description, f => f.Name.Random.Words(10));
    }

}