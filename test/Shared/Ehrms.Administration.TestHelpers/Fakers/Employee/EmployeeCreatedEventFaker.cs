using Bogus;
using Ehrms.Contracts.Employee;

namespace Ehrms.Administration.TestHelpers.Fakers.Employee;

public class EmployeeCreatedEventFaker : Faker<EmployeeCreatedEvent>
{
    public EmployeeCreatedEventFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.FirstName, f => f.Name.FirstName());
        RuleFor(x => x.LastName, f => f.Name.LastName());
    }
}