using Microsoft.EntityFrameworkCore;

namespace Ehrms.Administration.API.UnitTests.TestHelpers.Configurations;

internal static class DbContextFactory
{
    internal static AdministrationDbContext Create(string databaseName = "")
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            databaseName = Guid.NewGuid().ToString();
        }

        AdministrationDbContext dbContext = new(
            new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName)
                .Options);

        return dbContext;
    }
}