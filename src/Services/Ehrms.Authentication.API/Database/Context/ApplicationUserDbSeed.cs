using Microsoft.EntityFrameworkCore;
using Ehrms.Authentication.API.Database.Models;
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
        try
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();
            }
            await _userManager.CreateAsync(new User
            {
                UserName = "testUser",
                IsActive = true
            }, "Passw0rd!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not seed data");
        }
    }
}