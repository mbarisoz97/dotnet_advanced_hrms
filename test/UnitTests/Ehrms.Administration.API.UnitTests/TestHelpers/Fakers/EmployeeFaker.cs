using Bogus;
using Ehrms.Administration.API.Handlers.Payroll.Commands;
using Ehrms.Administration.API.Models;

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

internal class PaymentCriteriaFaker : Faker<PaymentCriteria>
{
    public PaymentCriteriaFaker()
    {
        RuleFor(x=>x.Id, Guid.NewGuid());
        RuleFor(x=>x.StartedAt, f => f.Date.FutureDateOnly());
    }

    public PaymentCriteriaFaker WithExpirationDate()
    {
        RuleFor(x => x.ExpiredAt, f => f.Date.FutureDateOnly());
        return this;
    }

    public PaymentCriteriaFaker WithEmployee(Employee employee)
    {
        RuleFor(x=>x.Employee, employee);
        return this;
    }
}

internal class CreatePayrollCommandFaker : Faker<CreatePayrollCommand>
{
    public CreatePayrollCommandFaker()
    {
        RuleFor(x=>x.EmployeeId, Guid.NewGuid());
    }
}