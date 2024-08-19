using Ehrms.Shared;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Ehrms.ProjectManagement.API.Database.Context;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers.ProjectController;

public abstract class ProjectManagementApiBaseIntegrationTests : IClassFixture<ProjectManagementWebApplicationFactory>
{
    protected readonly HttpClient client;
	protected readonly ProjectDbContext dbContext;

	protected ProjectManagementApiBaseIntegrationTests(ProjectManagementWebApplicationFactory factory)
    {
        client = factory.CreateClient();

		client = factory.CreateClient();
		var scope = factory.Services.CreateScope();
		dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();

		var request = new GenerateJwtRequest
        {
            Username = "TestUser",
        };
        var jwt = new JwtTokenHandler().Generate(request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
    }
}