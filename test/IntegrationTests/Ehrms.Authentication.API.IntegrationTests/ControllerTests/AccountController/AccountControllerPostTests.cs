using Ehrms.Shared;
using Ehrms.Authentication.API.Handlers.Auth.Commands;

namespace Ehrms.Authentication.API.IntegrationTests.ControllerTests.AccountController;

public class AccountControllerPostTests : AuthenticationApiBaseIntegrationTest
{
    public AccountControllerPostTests(AuthenticationWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Login_EmptyUsername_ReturnsBadRequest()
    {
        var loginRequest = new AuthenticateUserCommandFaker()
            .WithUsername("")
            .Generate();
        var response = await client.PostAsJsonAsync(AccountControllerEndpoints.Login,  loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Login_EmptyPassword_ReturnsBadRequest()
    {
        var loginRequest = new AuthenticateUserCommandFaker()
            .WithPassword("")
            .Generate();
        var response = await client.PostAsJsonAsync(AccountControllerEndpoints.Login,  loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Login_InactiveUserAccount_ReturnsForbidden()
    {
        var user = new UserFaker()
            .WithUserName("MyTestsUser")
            .WithAccountStatus(false)
            .Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var loginRequest = new AuthenticateUserCommandFaker()
            .WithUsername(user.UserName!)
            .Generate();

        var response = await client.PostAsJsonAsync(AccountControllerEndpoints.Login, loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Login_InvalidUserCredentials_ReturnsBadRequest()
    {
        var user = new UserFaker().Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var loginRequest = new AuthenticateUserCommandFaker()
            .WithUsername(user.UserName!)
            .Generate();

        var response = await client.PostAsJsonAsync(AccountControllerEndpoints.Login, loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Refresh_NonExistingUser_ReturnsBadRequest()
    {
        var request = new GenerateJwtRequest()
        {
            Username = "MyUserName",
            Roles = ["AnyRole"]
        };
        
        var accessToken = new JwtTokenHandler()
            .Generate(request)!.AccessToken;
        
        var loginRequest = new RefreshAuthenticationCommandFaker()
            .WithAccessToken(accessToken)
            .Generate();

        var response = await client.PostAsJsonAsync(AccountControllerEndpoints.Refresh, loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Refresh_InactiveUserAccount_ReturnsForbidden()
    {
        var user = new UserFaker()
            .WithUserName("MyTestUser")
            .WithAccountStatus(false)
            .Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var request = new GenerateJwtRequest()
        {
            Username = user.UserName!,
            Roles = ["AnyRole"]
        };
        
        var accessToken = new JwtTokenHandler()
            .Generate(request)!.AccessToken;
        
        var loginRequest = new RefreshAuthenticationCommandFaker()
            .WithAccessToken(accessToken)
            .WithRefreshToken(user.RefreshToken)
            .Generate();

        var response = await client.PostAsJsonAsync(AccountControllerEndpoints.Refresh, loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}