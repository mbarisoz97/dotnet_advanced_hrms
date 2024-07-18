using Ehrms.ProjectManagement.API.Database.Models;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker;

internal class ProjectFaker : Faker<Project>
{
    public ProjectFaker()
    {
		RuleFor(e => e.Id, f => f.Random.Guid());
		RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10));
        RuleFor(x => x.Description, f => f.Name.Random.AlphaNumeric(10));
    }

    public ProjectFaker WithEmployments(ICollection<Employment> employments)
    {
        RuleFor(x => x.Employments, employments);
        return this;
    }
}