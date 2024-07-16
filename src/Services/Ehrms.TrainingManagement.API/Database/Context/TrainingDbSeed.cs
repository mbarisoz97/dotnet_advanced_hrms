namespace Ehrms.TrainingManagement.API.Database.Context;

public class TrainingDbSeed
{
	private readonly TrainingDbContext _context;
	private readonly ILogger<TrainingDbSeed> _logger;

	public TrainingDbSeed(ILogger<TrainingDbSeed> logger, TrainingDbContext context)
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