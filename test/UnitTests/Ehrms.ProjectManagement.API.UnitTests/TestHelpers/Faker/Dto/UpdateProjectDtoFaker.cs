using Bogus;
using Ehrms.ProjectManagement.API.Dtos.Project;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker.Dto;
internal class UpdateProjectDtoFaker : Faker<UpdateProjectDto>
{
    public UpdateProjectDtoFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10));
        RuleFor(x => x.Description, f => f.Name.Random.AlphaNumeric(10));
    }
}