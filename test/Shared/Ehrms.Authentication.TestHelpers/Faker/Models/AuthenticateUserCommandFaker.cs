using Ehrms.Authentication.API.Handlers.Auth.Commands;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public class AuthenticateUserCommandFaker : Faker<AuthenticateUserCommand>
{
    public AuthenticateUserCommandFaker()
    {
        RuleFor(x => x.Username, f => f.Person.UserName);
        RuleFor(x => x.Password, f => f.Random.Chars(count: 10).ToString());
    }

    public AuthenticateUserCommandFaker WithUsername(string username)
    {
        RuleFor(x => x.Username, username);
        return this;
    }

    public AuthenticateUserCommandFaker WithPassword(string password)
    {
        RuleFor(x => x.Password, password);
        return this;
    }
}