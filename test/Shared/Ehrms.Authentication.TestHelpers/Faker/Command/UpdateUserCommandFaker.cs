using Ehrms.Authentication.API.Database.Models;

namespace Ehrms.Authentication.TestHelpers.Faker.Command;

public class UpdateUserCommandFaker : Faker<UpdateUserCommand>
{
    public UpdateUserCommandFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.IsActive, f => f.Random.Bool());
        RuleFor(x => x.Roles, f => [.. f.Random.WordsArray(2)]);
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

    public UpdateUserCommandFaker WithRoles(IEnumerable<UserRoles> userRoles)
    {
        RuleFor(x => x.Roles, userRoles.Select(x=>x.ToString()));
        return this;
    }

    public UpdateUserCommandFaker WithRoles(IEnumerable<Role> userRoles)
    {
        RuleFor(x => x.Roles, userRoles.Select(x => x.ToString()));
        return this;
    }
}