using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;

namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;

public sealed class TitleFaker : Faker<API.Database.Models.Title>
{
    public TitleFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.TitleName, f => f.Name.JobTitle());
    }

    public TitleFaker WithName(string name)
    {
        RuleFor(x => x.TitleName, name);
        return this;
    }
}
