using Ehrms.Shared;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Ehrms.ProjectManagement.API.Database.Context;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers.ProjectController;

[Collection(nameof(ProjectManagementWebApplicationFactory))]
public abstract class ProjectManagementApiBaseIntegrationTests : IAsyncLifetime
{
    protected readonly HttpClient client;
	protected readonly ProjectDbContext dbContext;
    private readonly Func<Task> _resetDatabase;

    protected ProjectManagementApiBaseIntegrationTests(ProjectManagementWebApplicationFactory factory)
    {
        client = factory.CreateClient();

		client = factory.CreateClient();
		var scope = factory.Services.CreateScope();
		dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
        _resetDatabase = factory.ResetDatabaseAsync;

        var request = new GenerateJwtRequest
        {
            Username = "TestUser",
        };
        var jwt = new JwtTokenHandler().Generate(request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
    }

    public Task InitializeAsync() => _resetDatabase();
    public Task DisposeAsync() => Task.CompletedTask;
}