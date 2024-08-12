using Ehrms.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Ehrms.Authentication.API.IntegrationTests.TestHelpers;

public abstract class AuthenticatipnApiBaseIntegrationTest : IClassFixture<AuthenticationWebApplicationFactory>
{
    protected readonly HttpClient client;
    protected readonly ApplicationUserDbContext dbContext;

    protected AuthenticatipnApiBaseIntegrationTest(AuthenticationWebApplicationFactory factory)
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