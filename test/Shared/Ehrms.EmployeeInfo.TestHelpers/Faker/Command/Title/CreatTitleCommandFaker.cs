namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Title;

public sealed class CreatTitleCommandFaker : Faker<CreateTitleCommand>
{
    public CreatTitleCommandFaker()
    {
        RuleFor(x => x.TitleName, f => f.Name.JobTitle());
    }

    public CreatTitleCommandFaker WithName(string name)
    {
        RuleFor(x => x.TitleName, name);
        return this;
    }
}