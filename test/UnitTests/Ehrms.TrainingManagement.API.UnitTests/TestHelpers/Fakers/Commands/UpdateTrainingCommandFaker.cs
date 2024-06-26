﻿namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers.Fakers.Commands;

internal class UpdateTrainingCommandFaker : Faker<UpdateTrainingCommand>
{
    public UpdateTrainingCommandFaker()
    {
        RuleFor(e => e.Name, f => f.Random.Word());
        RuleFor(e => e.PlannedAt, f => f.Date.Future());
        RuleFor(e => e.Description, f => f.Random.Words());
    }

    public UpdateTrainingCommandFaker WithId(Guid id)
    {
        RuleFor(e=>e.Id, id);
        return this;
    }
}
