using Ehrms.Shared;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public sealed class AuthenticationRequestFaker : Faker<AuthenticationRequest>
{
    public AuthenticationRequestFaker()
    {
        RuleFor(x=>x.Username, f => f.Person.UserName);
        RuleFor(x=>x.Password, f => f.Random.String(10,20));
        RuleFor(x => x.Roles, f => f.Random.WordsArray(1));
    }
    
    public AuthenticationRequestFaker WithUserName(string username)
    {
        RuleFor(x=>x.Username, username);
        return this;
    }

    public AuthenticationRequestFaker WithPassword(string password)
    {
        RuleFor(x => x.Password, password);
        return this;
    }

    public AuthenticationRequestFaker WithRoles(ICollection<string> userRoles)
    {
        RuleFor(x => x.Roles, userRoles);
        return this;
    }
}