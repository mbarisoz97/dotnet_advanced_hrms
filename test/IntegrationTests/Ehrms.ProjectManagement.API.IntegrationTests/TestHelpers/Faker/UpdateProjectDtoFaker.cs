﻿using Bogus;

namespace Ehrms.ProjectManagement.API.IntegrationTests.TestHelpers.Faker;

internal class UpdateProjectDtoFaker : Faker<UpdateProjectDto>
{
    public UpdateProjectDtoFaker()
    {
        RuleFor(x => x.Name, f => f.Name.Random.AlphaNumeric(10));
        RuleFor(x => x.Description, f => f.Name.Random.AlphaNumeric(10));
    }
}