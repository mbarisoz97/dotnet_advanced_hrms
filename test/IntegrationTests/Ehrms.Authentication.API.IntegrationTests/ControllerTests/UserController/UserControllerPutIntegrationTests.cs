using System.Net;
using System.Net.Http.Json;

namespace Ehrms.Authentication.API.IntegrationTests.ControllerTests.UserController;

public class UserControllerPutIntegrationTests : AuthenticatipnApiBaseIntegrationTest
{
    public UserControllerPutIntegrationTests(AuthenticationWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Register_InvalidEmailFormat_ReturnsBadRequest()
    {
        var command = new RegisterUserCommandFaker()
            .WithEmail("anyInvalidEmail")
            .Generate();

        var response = await client.PutAsJsonAsync(UserControllerEndpoints.Register, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShortUsername_ReturnsBadRequest()
    {
        var command = new RegisterUserCommandFaker()
            .WithPassword("a")
            .Generate();

        var response = await client.PutAsJsonAsync(UserControllerEndpoints.Register, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_LongUserName_ReturnsBadRequest()
    {
        var command = new RegisterUserCommandFaker()
            .WithPassword("a")
            .Generate();

        var response = await client.PutAsJsonAsync(UserControllerEndpoints.Register, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ShortPassword_ReturnsBadRequest()
    {
        var command = new RegisterUserCommandFaker()
            .WithPassword("asdbcdf")
            .Generate();

        var response = await client.PutAsJsonAsync(UserControllerEndpoints.Register, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_LongPassword_ReturnsBadRequest()
    {
        var command = new RegisterUserCommandFaker()
            .WithPassword(new string('s', 51))
            .Generate();

        var response = await client.PutAsJsonAsync(UserControllerEndpoints.Register, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ValidUserRegisteryDetails_ReturnsOk()
    {
        var command = new RegisterUserCommandFaker().Generate();
        var response = await client.PutAsJsonAsync(UserControllerEndpoints.Register, command);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}