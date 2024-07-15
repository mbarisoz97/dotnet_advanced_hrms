namespace Ehrms.ProjectManagement.API.Database.Context;

public class ProjectManagementDatabaseSeed
{
	private readonly ProjectDbContext _context;
	private readonly ILogger<ProjectManagementDatabaseSeed> _logger;

	public ProjectManagementDatabaseSeed(ILogger<ProjectManagementDatabaseSeed> logger, ProjectDbContext context)
	{
		_logger = logger;
		_context = context;
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