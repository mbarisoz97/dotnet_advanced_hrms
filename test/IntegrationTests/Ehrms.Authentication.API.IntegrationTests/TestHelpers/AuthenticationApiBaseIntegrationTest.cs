using Ehrms.Shared;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Ehrms.Authentication.API.Database.Models;
using Ehrms.Authentication.API.Handlers.User.Commands;

namespace Ehrms.Authentication.API.IntegrationTests.TestHelpers;

[Collection(nameof(AuthenticationWebApplicationFactory))]
public abstract class AuthenticationApiBaseIntegrationTest :IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;

    protected readonly HttpClient client;
    protected readonly ApplicationUserDbContext dbContext;

    protected AuthenticationApiBaseIntegrationTest(AuthenticationWebApplicationFactory factory)
    {
        client = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<ApplicationUserDbContext>();
        _resetDatabase = factory.ResetDatabaseAsync;

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

    public Task InitializeAsync() => _resetDatabase();
    public Task DisposeAsync() => Task.CompletedTask;

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