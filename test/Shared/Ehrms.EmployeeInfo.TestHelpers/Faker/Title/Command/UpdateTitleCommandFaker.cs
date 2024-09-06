namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;

public sealed class UpdateTitleCommandFaker : Faker<UpdateTitleCommand>
{
    public UpdateTitleCommandFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.TitleName, f => f.Name.JobTitle());
    }
    
    public UpdateTitleCommandFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }

    public UpdateTitleCommandFaker WithName(string name)
    {
        RuleFor(x => x.TitleName, name);
        return this;
    }
}