using Microsoft.EntityFrameworkCore;

namespace Ehrms.TrainingManagement.API.UnitTests.Consumers.Training;

public abstract class UnitTestsBase<T>(T dbContext) : IAsyncLifetime
    where T : DbContext
{
    protected readonly T dbContext = dbContext;

    public async Task InitializeAsync()
    {
        await dbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await dbContext.Database.EnsureDeletedAsync();
    }
}