using Bogus;
using Ehrms.Contracts.Employee;

namespace Ehrms.Administration.TestHelpers.Fakers.Employee;

public class EmployeeUpdatedEventFaker : Faker<EmployeeUpdatedEvent>
{
    public EmployeeUpdatedEventFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.FirstName, f => f.Name.FirstName());
        RuleFor(x => x.LastName, f => f.Name.LastName());
    }

    public EmployeeUpdatedEventFaker WithId(Guid id )
    {
        RuleFor(x => x.Id, id);
        return this;
    }

    public EmployeeUpdatedEventFaker WithFirstName(string firstName)
    {
        RuleFor(x => x.FirstName, firstName);
        return this;
    }

    public EmployeeUpdatedEventFaker WithLastName(string lastName)
    {
        RuleFor(x => x.LastName, lastName);
        return this;
    }
}