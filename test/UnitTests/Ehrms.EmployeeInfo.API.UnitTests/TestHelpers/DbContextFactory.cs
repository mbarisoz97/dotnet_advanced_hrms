using Ehrms.EmployeeInfo.API.Database.Context;

namespace Ehrms.EmployeeInfo.API.UnitTests.TestHelpers;

internal static class DbContextFactory
{
    internal static EmployeeInfoDbContext Create(string databaseName)
    {
        EmployeeInfoDbContext dbContext = new(
            new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName)
                .Options);

        return dbContext;
    }
}