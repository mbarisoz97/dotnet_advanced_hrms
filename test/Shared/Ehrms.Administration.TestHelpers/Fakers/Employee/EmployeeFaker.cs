using Bogus;

namespace Ehrms.Administration.TestHelpers.Fakers.Employee;

public class EmployeeFaker : Faker<API.Database.Models.Employee>
{
    public EmployeeFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.FirstName, f => f.Name.FirstName());
        RuleFor(x => x.LastName, f => f.Name.LastName());
    }
}