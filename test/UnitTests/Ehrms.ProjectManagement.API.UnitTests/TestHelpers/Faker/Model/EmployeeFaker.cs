using Ehrms.ProjectManagement.API.Database.Models;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers.Faker;

internal class EmployeeFaker : Faker<Employee>
{
    public EmployeeFaker()
    {
        RuleFor(e => e.Id, f => f.Random.Guid());
        RuleFor(e => e.FirstName, f => f.Name.FirstName());
        RuleFor(e => e.LastName, f => f.Name.LastName());
    }
}
