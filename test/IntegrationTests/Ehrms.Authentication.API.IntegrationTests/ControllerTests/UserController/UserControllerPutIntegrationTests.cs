using Ehrms.Authentication.API.Dto.User;
using Ehrms.Authentication.API.Handlers.User.Commands;
using System.Net;
using System.Net.Http.Json;

namespace Ehrms.Authentication.API.IntegrationTests.ControllerTests.UserController;

public class UserControllerPutIntegrationTests : AuthenticationApiBaseIntegrationTest
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
    public async Task Register_NoUserRoles_ReturnsBadRequest()
    {
        var command = new RegisterUserCommandFaker()
            .WithRoles([])
            .Generate();

        var response = await client.PutAsJsonAsync(UserControllerEndpoints.Register, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_ValidUserRegisteryDetails_ReturnsOk()
    {
        var command = new RegisterUserCommandFaker()
            .WithRoles([UserRoles.Admin, UserRoles.User])
            .Generate();
        var response = await client.PutAsJsonAsync(UserControllerEndpoints.Register, command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var readUserDto = await response.Content.ReadFromJsonAsync<RegisterUserResponseDto>();
        readUserDto.Should().BeEquivalentTo(command, opt =>
            opt.Excluding(p => p.Roles)
               .Excluding(p => p.Password));
    }
}