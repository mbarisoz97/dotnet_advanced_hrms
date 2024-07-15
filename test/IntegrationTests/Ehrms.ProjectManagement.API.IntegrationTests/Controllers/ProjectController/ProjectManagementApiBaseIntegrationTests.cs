using Ehrms.Shared;
using System.Net.Http.Headers;
using Ehrms.ProjectManagement.API.Context;
using Microsoft.Extensions.DependencyInjection;

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

		var request = new AuthenticationRequest
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        var jwt = new JwtTokenHandler().Generate(request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
    }
}