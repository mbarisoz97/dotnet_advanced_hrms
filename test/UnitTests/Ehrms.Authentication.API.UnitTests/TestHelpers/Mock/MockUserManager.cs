using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.TestHelpers.Faker.Models;
using LanguageExt;

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

    public void SetupGeneratePasswordResetTokenAsync(string? token = null)
    {
        Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>()))!
            .ReturnsAsync(token);
    }
    
    public void SetupResetPasswordAsync(IdentityResult identityResult)
    {
        Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);
    }
}