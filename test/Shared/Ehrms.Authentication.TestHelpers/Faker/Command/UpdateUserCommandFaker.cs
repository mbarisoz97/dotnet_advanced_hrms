namespace Ehrms.Authentication.TestHelpers.Faker.Command;

public class UpdateUserCommandFaker : Faker<UpdateUserCommand>
{
    public UpdateUserCommandFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.IsActive, f => f.Random.Bool());
    }

    public UpdateUserCommandFaker WithId(Guid id)
    {
        RuleFor(x => x.Id, id);
        return this;
    }

    public UpdateUserCommandFaker WithAccountStatus(bool isActive)
    {
        RuleFor(x => x.IsActive, isActive);
        return this;
    }
}