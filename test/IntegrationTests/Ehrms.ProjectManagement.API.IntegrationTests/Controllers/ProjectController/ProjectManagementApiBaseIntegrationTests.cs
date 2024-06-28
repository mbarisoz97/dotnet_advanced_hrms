using Ehrms.Shared;
using System.Net.Http.Headers;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers.ProjectController;

public abstract class ProjectManagementApiBaseIntegrationTests : IClassFixture<ProjectManagementWebApplicationFactory>
{
    protected readonly ProjectManagementWebApplicationFactory _factory;
    protected readonly HttpClient _client;

    protected ProjectManagementApiBaseIntegrationTests(ProjectManagementWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();

        var request = new AuthenticationRequest
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        var jwt = new JwtTokenHandler().Generate(request);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
        _factory = factory;
    }
}