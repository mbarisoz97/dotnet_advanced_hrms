using Ehrms.TrainingManagement.API.Context;
using Microsoft.EntityFrameworkCore;

namespace Ehrms.TrainingManagement.API.UnitTests.TestHelpers;

internal static class CustomDbContextFactory
{
	internal static TrainingDbContext CreateWithInMemoryDatabase(string databaseName)
	{
		TrainingDbContext projectDbContext = new(new DbContextOptionsBuilder<TrainingDbContext>()
			.UseInMemoryDatabase(databaseName)
			.Options);

		return projectDbContext;
	}
}