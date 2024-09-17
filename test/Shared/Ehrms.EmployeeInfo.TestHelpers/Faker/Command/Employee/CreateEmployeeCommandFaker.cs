﻿using Ehrms.EmployeeInfo.API.Dtos.Employee;

namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Employee;

public class CreateEmployeeCommandFaker : Faker<CreateEmployeeCommand>
{
    public CreateEmployeeCommandFaker()
    {
        RuleFor(p => p.FirstName, f => f.Name.FirstName());
        RuleFor(p => p.LastName, f => f.Name.LastName());
        RuleFor(e => e.DateOfBirth, f => f.Date.BetweenDateOnly(new DateOnly(2000, 1, 1), new DateOnly(2022, 12, 1)));
    }

    public CreateEmployeeCommandFaker WithTitleId(Guid id)
    {
        RuleFor(e=>e.Title, new ReadTitleDto()
        {
            Id = id,
        });
        return this;
    }
}