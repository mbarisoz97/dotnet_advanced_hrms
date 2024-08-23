using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API.Database.Context;

public class ApplicationUserDbSeed
{
    private readonly ILogger<ApplicationUserDbSeed> _logger;
    private readonly ApplicationUserDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public ApplicationUserDbSeed(ILogger<ApplicationUserDbSeed> logger,
        ApplicationUserDbContext context,
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        var user = new User
        {
            UserName = "testUser",
            Email = "adminTestAccount@test.com",
            IsActive = true,
            MustChangePassword = false,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var existingUser = await _userManager.FindByNameAsync(user.UserName);
        if (existingUser != null)
        {
            return;
        }

        await CreateAdminTestUser(user, "Passw0rd!");
        await AddAdminUserRole(user);
    }

    private async Task AddAdminUserRole(User user)
    {
        var adminRole = await _roleManager.FindByNameAsync(UserRoles.Admin.ToString());
        if (adminRole == null)
        {
            _logger.LogError("Could not find admin role for test user.");
            return;
        }

        var userRole = new UserRole
        {
            User = user,
            Role = adminRole
        };

        await _context.AddAsync(userRole);
        await _context.SaveChangesAsync();
    }

    private async Task CreateAdminTestUser(User user, string password)
    {
        if (user == null)
        {
            _logger.LogError("Could not add null user");
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