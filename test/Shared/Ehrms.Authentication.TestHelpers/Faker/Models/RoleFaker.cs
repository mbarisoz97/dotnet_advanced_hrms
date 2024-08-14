using Ehrms.Authentication.API.Database.Models;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public class RoleFaker : Faker<Role>
{
    public RoleFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Name, f => f.Random.Word());
    }

    public RoleFaker WithName(string name)
    {
        RuleFor(x => x.Name, name);
        return this;
    }
}