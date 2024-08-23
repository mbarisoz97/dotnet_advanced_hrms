using Ehrms.Authentication.API.Database.Models;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public sealed class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(u => u.IsActive, true);
        RuleFor(u => u.MustChangePassword, false);
        RuleFor(u => u.Id, f => f.Random.Guid());
        RuleFor(u => u.Email, f => f.Person.Email);
        RuleFor(u => u.UserName, f => f.Person.UserName);
        RuleFor(u => u.NormalizedUserName, (_, u) => u.UserName?.Normalize());
        RuleFor(x => x.SecurityStamp, f => f.Random.Guid().ToString());
        RuleFor(x => x.RefreshToken, f => f.Random.AlphaNumeric(20));
        RuleFor(x => x.RefreshTokenExpiry, f => f.Date.Future());
    }

    public UserFaker WithUserName(string username)
    {
        RuleFor(x => x.UserName, username);
        RuleFor(x => x.NormalizedUserName, username.Normalize());
        return this;
    }

    public UserFaker WithPasswordStatus(bool mustChangePassword)
    {
        RuleFor(x => x.MustChangePassword, mustChangePassword);
        return this;
    }

    public UserFaker WithAccountStatus(bool isActive)
    {
        RuleFor(x => x.IsActive, isActive);
        return this;
    }
}