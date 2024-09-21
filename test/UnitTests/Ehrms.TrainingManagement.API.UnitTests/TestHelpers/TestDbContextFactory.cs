using Microsoft.EntityFrameworkCore;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers;

internal static class TestDbContextFactory
{
	internal static TrainingDbContext CreateDbContext(string databaseName = "")
	{
		if (string.IsNullOrEmpty(databaseName))
		{
			databaseName = Guid.NewGuid().ToString();
		}

		TrainingDbContext projectDbContext = new(new DbContextOptionsBuilder<TrainingDbContext>()
			.UseInMemoryDatabase(databaseName)
			.Options);

		return projectDbContext;
	}
}