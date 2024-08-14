using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public class UserRoleFaker : Faker<IdentityUserRole<Guid>>
{
    public UserRoleFaker WithUserId(Guid id)
    {
        RuleFor(x => x.UserId, id);
        return this;
    }
    public UserRoleFaker WithRoleId(Guid id)
    {
        RuleFor(x => x.RoleId, id);
        return this;
    }
}