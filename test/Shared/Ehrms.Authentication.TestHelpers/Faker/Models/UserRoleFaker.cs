using Ehrms.Authentication.API.Database.Models;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public class UserRoleFaker : Faker<UserRole>
{
    public UserRoleFaker WithUser(User user)
    {
        RuleFor(p => p.UserId, user.Id);
        RuleFor(p => p.User, user);
        return this;
    }

    public UserRoleFaker WithRole(Role role)
    {
        RuleFor(p => p.RoleId, role.Id);
        RuleFor(p => p.Role, role);
        return this;
    }
}