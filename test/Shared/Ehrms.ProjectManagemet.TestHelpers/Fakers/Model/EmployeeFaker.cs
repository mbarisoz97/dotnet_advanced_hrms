using Bogus;
using Ehrms.ProjectManagement.API.Database.Models;

namespace Ehrms.ProjectManagement.API.TestHelpers.Faker;

public class EmployeeFaker : Faker<Employee>
{
    public EmployeeFaker()
    {
        RuleFor(e => e.FirstName, f => f.Name.FirstName());
        RuleFor(e => e.LastName, f => f.Name.LastName());
    }
}
