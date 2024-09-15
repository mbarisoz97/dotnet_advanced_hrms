using Bogus;
using Ehrms.Administration.API.Database.Models;

namespace Ehrms.Administration.API.UnitTests.TestHelpers.Fakers;

internal class EmployeeFaker : Faker<Employee>
{
    public EmployeeFaker()
    {
        RuleFor(x => x.Id, Guid.NewGuid());
        RuleFor(x => x.FirstName, f => f.Name.FirstName());
        RuleFor(x => x.LastName, f => f.Name.LastName());
    }
}