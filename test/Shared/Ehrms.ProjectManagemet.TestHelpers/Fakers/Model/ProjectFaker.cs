using Ehrms.ProjectManagement.API.Models;

namespace Ehrms.ProjectManagement.API.TestHelpers.Faker;

public class ProjectFaker : Faker<Project>
{
    public ProjectFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10));
        RuleFor(x => x.Description, f => f.Name.Random.AlphaNumeric(10));
    }

    public ProjectFaker WithEmployments(ICollection<Employment> employments)
    {
        RuleFor(x => x.Employments, employments);
        return this;
    }
}