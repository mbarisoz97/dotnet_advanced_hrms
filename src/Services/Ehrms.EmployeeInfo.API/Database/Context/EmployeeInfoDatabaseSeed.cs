namespace Ehrms.EmployeeInfo.API.Database.Context;

public class EmployeeInfoDatabaseSeed
{
	private readonly ILogger<EmployeeInfoDatabaseSeed> _logger;
	private readonly EmployeeInfoDbContext _context;

	public EmployeeInfoDatabaseSeed(ILogger<EmployeeInfoDatabaseSeed> logger, EmployeeInfoDbContext dbContext)
	{
		_logger = logger;
		_context = dbContext;
	}

	public async Task SeedAsync()
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
			_logger.LogError(e, "Could not seed data");
		}
	}
}