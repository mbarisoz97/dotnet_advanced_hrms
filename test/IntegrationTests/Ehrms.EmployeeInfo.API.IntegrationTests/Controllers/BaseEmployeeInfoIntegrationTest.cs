using Ehrms.Shared;
using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

[Collection(nameof(EmployeeInfoWebApplicationFactory))]
public abstract class BaseEmployeeInfoIntegrationTest : IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;

    protected readonly EmployeeInfoDbContext dbContext;
    protected readonly EmployeeInfoWebApplicationFactory factory;
    protected readonly HttpClient client;

    protected BaseEmployeeInfoIntegrationTest(EmployeeInfoWebApplicationFactory factory)
    {
        this.factory = factory;
        client = this.factory.CreateClient();

        var scope = factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<EmployeeInfoDbContext>();
        _resetDatabase = factory.ResetDatabaseAsync;

        var request = new GenerateJwtRequest
        {
            Username = "TestUser"
        };
        var jwt = new JwtTokenHandler().Generate(request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
        this.factory = factory;
    }

    public Task InitializeAsync() => _resetDatabase();
    public Task DisposeAsync() => Task.CompletedTask;
}