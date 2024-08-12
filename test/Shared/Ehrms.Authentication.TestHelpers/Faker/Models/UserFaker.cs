using Ehrms.Authentication.API.Database.Models;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public class UserFaker : Faker<User>
{
    public UserFaker()
    {
        RuleFor(x => x.UserName, f => f.Person.UserName);
        RuleFor(x => x.Email, f => f.Person.Email);
    }
}
