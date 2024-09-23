using Ehrms.Administration.API.Database.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Configurations;

[Collection(nameof(AdministrationWebApplicationFactory))]
public abstract class AdministrationApiBaseIntegrationTest : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    protected readonly HttpClient client;
    protected readonly AdministrationDbContext dbContext;

    protected AdministrationApiBaseIntegrationTest(AdministrationWebApplicationFactory factory)
    {
        client = factory.CreateClient();
        _resetDatabase = factory.ResetDatabaseAsync;
        var scope = factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<AdministrationDbContext>();
    }


    public Task InitializeAsync() => _resetDatabase();
    public Task DisposeAsync() => Task.CompletedTask;
}