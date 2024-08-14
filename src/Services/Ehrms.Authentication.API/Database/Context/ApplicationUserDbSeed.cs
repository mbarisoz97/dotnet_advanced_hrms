using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Database.Context;

public class ApplicationUserDbSeed
{
    private readonly ILogger<ApplicationUserDbSeed> _logger;
    private readonly ApplicationUserDbContext _context;
    private readonly UserManager<User> _userManager;

    public ApplicationUserDbSeed(ILogger<ApplicationUserDbSeed> logger, ApplicationUserDbContext context, UserManager<User> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        var user = new User
        {
            UserName = "testUser",
            Email = "adminTestAccount@test.com",
            IsActive = true,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        await CreateAdminTestUser(user, "Passw0rd!");
        await AddAdminUserRole(user);
    }

    private async Task AddAdminUserRole(User user)
    {
        var assignRoleResult = await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
        if (!assignRoleResult.Succeeded)
        {
            foreach (var error in assignRoleResult.Errors)
            {
                _logger.LogError(error.Description);
            }
        }
    }

    private async Task CreateAdminTestUser(User user, string password)
    {
        if (user == null)
        {
            _logger.LogError("Could not add null user");
        }

        var existingUser = await _userManager.FindByNameAsync(user.UserName);
        if (existingUser != null)
        {
            return;
        }

        var identityResult = await _userManager.CreateAsync(user, password);
        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                _logger.LogError(error.Description);
            }
        }
    }
}