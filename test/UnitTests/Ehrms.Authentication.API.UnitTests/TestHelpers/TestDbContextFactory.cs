using Microsoft.EntityFrameworkCore;
using Ehrms.Authentication.API.Database.Context;

namespace Ehrms.Authentication.API.UnitTests.TestHelpers;

internal static class TestDbContextFactory
{
    internal static ApplicationUserDbContext CreateDbContext(string databaseName)
    {
        ApplicationUserDbContext projectDbContext = new(new DbContextOptionsBuilder<ApplicationUserDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options);

        return projectDbContext;
    }
}
