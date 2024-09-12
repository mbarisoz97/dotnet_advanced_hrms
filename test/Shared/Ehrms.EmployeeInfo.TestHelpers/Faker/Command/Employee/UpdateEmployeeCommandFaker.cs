namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Employee;

public class UpdateEmployeeCommandFaker : Faker<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandFaker()
    {
        RuleFor(p => p.Id, f => f.Random.Guid());
        RuleFor(p => p.FirstName, f => f.Name.FirstName());
        RuleFor(p => p.LastName, f => f.Name.LastName());
        RuleFor(e => e.DateOfBirth, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
        RuleFor(e => e.Qualification, f => f.Name.JobTitle());
    }

    public UpdateEmployeeCommandFaker WithId(Guid id)
    {
        RuleFor(p => p.Id, id);
        return this;
    }

    public UpdateEmployeeCommandFaker WithTitleId(Guid id)
    {
        RuleFor(x => x.TitleId, id);
        return this;
    }

    public UpdateEmployeeCommandFaker WithSkills(ICollection<Guid> skills)
    {
        RuleFor(x => x.Skills, skills);
        return this;
    }

    public UpdateEmployeeCommandFaker WithSkills(ICollection<API.Database.Models.Skill> skills)
    {
        RuleFor(x => x.Skills, skills.Select(x=>x.Id).ToArray());
        return this;
    }
}