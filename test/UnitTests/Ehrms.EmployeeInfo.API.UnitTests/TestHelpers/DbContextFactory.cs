using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.UnitTests.TestHelpers;

internal static class DbContextFactory
{
    internal static EmployeeInfoDbContext Create(string databaseName = "")
    {
        if (string.IsNullOrEmpty(databaseName))
        {
            databaseName = Guid.NewGuid().ToString();
        }

        EmployeeInfoDbContext dbContext = new(
            new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName)
                .Options);

        return dbContext;
    }
}