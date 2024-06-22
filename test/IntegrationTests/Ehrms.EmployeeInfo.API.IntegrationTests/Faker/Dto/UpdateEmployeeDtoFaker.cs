﻿namespace Ehrms.EmployeeInfo.API.IntegrationTests.Faker.Dto;

internal class UpdateEmployeeDtoFaker : Faker<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoFaker()
    {
        RuleFor(e => e.FirstName, f => f.Name.FirstName());
        RuleFor(e => e.LastName, f => f.Name.LastName());
        RuleFor(e => e.DateOfBirth, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
        RuleFor(e => e.Qualification, f => f.Name.JobTitle());
    }
}