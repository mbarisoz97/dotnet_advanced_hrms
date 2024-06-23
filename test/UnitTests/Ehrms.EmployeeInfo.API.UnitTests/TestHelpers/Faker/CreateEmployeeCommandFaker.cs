﻿namespace Ehrms.EmployeeInfo.API.UnitTests.TestHelpers.Faker;

internal class CreateEmployeeCommandFaker : Faker<CreateEmployeeCommand>
{
    public CreateEmployeeCommandFaker()
    {
        RuleFor(p => p.FirstName, f => f.Name.FirstName());
        RuleFor(p => p.LastName, f => f.Name.LastName());
        RuleFor(e => e.DateOfBirth, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
        RuleFor(e => e.Qualification, f => f.Name.JobTitle());
    }
}