using Bogus;
using Ehrms.EmployeeInfo.API.Dtos.Employee;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Fakers.Dto;

internal class CreateEmployeeDtoFaker : Faker<CreateEmployeeDto>
{
    public CreateEmployeeDtoFaker()
    {
        RuleFor(e => e.FirstName, f => f.Name.FirstName());
        RuleFor(e => e.LastName, f => f.Name.LastName());
        RuleFor(e => e.DateOfBirth, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
        RuleFor(e => e.Qualification, f => f.Name.JobTitle());
    }
}