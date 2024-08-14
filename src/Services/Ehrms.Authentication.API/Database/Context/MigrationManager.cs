using Microsoft.EntityFrameworkCore;
using LanguageExt;

namespace Ehrms.Authentication.API.Database.Context;

public class MigrationManager
{
    private readonly ApplicationUserDbContext _context;
    private readonly ILogger<ApplicationUserDbSeed> _logger;

    public MigrationManager(ILogger<ApplicationUserDbSeed> logger, ApplicationUserDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Init()
    {
        if (_context.Database.GetPendingMigrations().Any())
        {
            await _context.Database.MigrateAsync();
        }
    }
}
