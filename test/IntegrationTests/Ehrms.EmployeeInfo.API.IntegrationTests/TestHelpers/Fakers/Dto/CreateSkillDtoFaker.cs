﻿using Bogus;
using Ehrms.EmployeeInfo.API.Dtos.Skill;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Fakers.Dto;

internal class CreateSkillDtoFaker : Faker<CreateSkillDto>
{
    public CreateSkillDtoFaker()
    {
        RuleFor(e => e.Name, f => f.Name.Random.AlphaNumeric(6));
    }
}