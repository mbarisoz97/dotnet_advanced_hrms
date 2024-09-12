namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Model;

public sealed class TitleFaker : Faker<Title>
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
