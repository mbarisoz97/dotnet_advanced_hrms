namespace Ehrms.Administration.API.Database.Context;

public class MigrationManager
{
    private readonly AdministrationDbContext _context;
    private readonly ILogger<AdministrationDbContext> _logger;

    public MigrationManager(ILogger<AdministrationDbContext> logger, AdministrationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task Init()
    {
        try
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}
