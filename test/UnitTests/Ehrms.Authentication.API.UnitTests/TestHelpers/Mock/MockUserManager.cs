using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.TestHelpers.Faker.Models;

namespace Ehrms.Authentication.API.UnitTests.TestHelpers.Mock;

public sealed class MockUserManager : Mock<IUserManagerAdapter>
{
    private readonly ApplicationUserDbContext _dbContext;

    public MockUserManager(ApplicationUserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SetupCheckPasswordAsync(bool isPasswordTrue)
    {
        Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(isPasswordTrue);
    }

    public void SetupFindByNameAsync(User? user = null)
    {
        Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
    }

    public void SetupUpdateAsync(IdentityResult identityResult)
    {
        Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(identityResult);
    }

    public void SetupRemoveFromRolesAsync(IdentityResult identityResult)
    {
        Setup(x => x.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
              .Callback(async (User user, IEnumerable<string> roleNameCollection) =>
              {
                  await MockRemoveRolesAsync(user, roleNameCollection);
              })
             .ReturnsAsync(identityResult);
    }
    private async Task MockRemoveRolesAsync(User user, IEnumerable<string> roleNameCollection)
    {
        foreach (var roleName in roleNameCollection)
        {
            var role = _dbContext.Roles!.FirstOrDefault(r => r.Name == roleName);
            if (role == null)
            {
                continue;
            }

            var userRole = _dbContext.UserRoles.FirstOrDefault(x => x.UserId == user.Id && x.RoleId == role.Id);
            if (userRole != null)
            {
                user.UserRoles.Remove(userRole);
            }
        }
        await _dbContext.SaveChangesAsync();
    }


    public void SetupAddToRolesAsync(IdentityResult identityResult)
    {
        Setup(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .Callback(async (User user, IEnumerable<string> roleNameCollection) =>
            {
                await MockAddRolesToUser(user, roleNameCollection);
            })
            .ReturnsAsync(IdentityResult.Success);
    }
    private async Task MockAddRolesToUser(User user, IEnumerable<string> roleNameCollection)
    {
        foreach (var roleName in roleNameCollection)
        {
            var role = new RoleFaker()
                .WithName(roleName)
                .Generate();
            await _dbContext.AddAsync(role);

            var userRole = new UserRoleFaker()
                .WithUser(user)
                .WithRole(role)
                .Generate();

            await _dbContext.AddAsync(userRole);
        }
        await _dbContext.SaveChangesAsync();
    }
}