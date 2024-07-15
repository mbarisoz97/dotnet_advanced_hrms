using Ehrms.ProjectManagement.API.Database.Context;

namespace Ehrms.ProjectManagement.API.UnitTests.TestHelpers;

internal static class CustomDbContextFactory
{
    internal static ProjectDbContext CreateWithInMemoryDatabase(string databaseName)
    {
        ProjectDbContext projectDbContext = new(new DbContextOptionsBuilder<ProjectDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options);

        return projectDbContext;
    }
}