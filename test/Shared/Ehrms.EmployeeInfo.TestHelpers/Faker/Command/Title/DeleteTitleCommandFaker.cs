﻿namespace Ehrms.EmployeeInfo.TestHelpers.Faker.Command.Title;

public sealed class DeleteTitleCommandFaker : Faker<DeleteTitleCommand>
{
    public DeleteTitleCommandFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
    }

    public DeleteTitleCommandFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }
}