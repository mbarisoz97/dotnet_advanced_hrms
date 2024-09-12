using Ehrms.EmployeeInfo.API.Database.Models;

namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Model;

public class EmployeeFaker : Faker<Employee>
{
    public EmployeeFaker()
    {
        RuleFor(e => e.FirstName, f => f.Name.FirstName());
        RuleFor(e => e.LastName, f => f.Name.LastName());
        RuleFor(e => e.DateOfBirth, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
        RuleFor(e => e.Qualification, f => f.Name.JobTitle());
    }

    public EmployeeFaker WithSkills(ICollection<Skill> skills)
    {
        RuleFor(e => e.Skills, skills);
        return this;
    }

    public EmployeeFaker WithTitle(Title title)
    {
        RuleFor(e => e.Title, title);
        return this;
    }
}
