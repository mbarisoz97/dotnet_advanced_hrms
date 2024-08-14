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
            await _dbContext.Roles.AddAsync(role);
            user.Roles.Add(role);
        }
        await _dbContext.SaveChangesAsync();
    }
    private async Task MockRemoveRolesAsync(User user, IEnumerable<string> roleNameCollection)
    {
        foreach (var roleName in roleNameCollection)
        {
            var role = _dbContext.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                user.Roles.Remove(role);
            }
        }
        await _dbContext.SaveChangesAsync();
    }
}