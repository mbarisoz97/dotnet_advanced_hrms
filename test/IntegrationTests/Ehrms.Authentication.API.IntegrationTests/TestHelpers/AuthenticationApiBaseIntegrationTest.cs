using Ehrms.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Ehrms.Authentication.API.Database.Models;
using Ehrms.Authentication.API.Handlers.User.Commands;

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

        SetDefaultClientForAuthorizedAndAuthenticatedUser();
    }

    private void SetDefaultClientForAuthorizedAndAuthenticatedUser()
    {
        var request = new AuthenticationRequestFaker()
            .WithUserName("TestUser")
            .WithRoles(["Admin"])
            .Generate();

        var jwt = new JwtTokenHandler().Generate(request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
    }

    protected void SetClientForUserWithRoles(ICollection<UserRoles> userRoles)
    {
        var request = new AuthenticationRequestFaker()
            .WithRoles(userRoles.Select(x=>x.ToString()).ToList())
            .Generate();

        var jwt = new JwtTokenHandler().Generate(request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
    }
    
    protected async Task<User> AddRandomUser()
    {
        var user = new UserFaker().Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user;
    }
}