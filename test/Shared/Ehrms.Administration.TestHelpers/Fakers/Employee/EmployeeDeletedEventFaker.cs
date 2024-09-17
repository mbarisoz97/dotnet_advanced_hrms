using Bogus;
using Ehrms.Contracts.Employee;

namespace Ehrms.Administration.TestHelpers.Fakers.Employee;

public class EmployeeDeletedEventFaker : Faker<EmployeeDeletedEvent>
{
    public EmployeeDeletedEventFaker()
    {
    }

    public EmployeeDeletedEventFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }
}