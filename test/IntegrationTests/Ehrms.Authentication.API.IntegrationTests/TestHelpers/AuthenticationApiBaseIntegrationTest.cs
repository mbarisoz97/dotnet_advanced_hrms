using Ehrms.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Ehrms.Authentication.API.IntegrationTests.TestHelpers;

public abstract class AuthenticationApiBaseIntegrationTest : IClassFixture<AuthenticationWebApplicationFactory>
{
    protected readonly HttpClient client;
    protected readonly ApplicationUserDbContext dbContext;

    protected AuthenticationApiBaseIntegrationTest(AuthenticationWebApplicationFactory factory)
    {
        client = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<ApplicationUserDbContext>();

        var request = new AuthenticationRequest
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        var jwt = new JwtTokenHandler().Generate(request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
    }
}