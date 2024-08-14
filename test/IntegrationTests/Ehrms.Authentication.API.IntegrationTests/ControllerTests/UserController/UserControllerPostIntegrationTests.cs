using Ehrms.Authentication.API.Dto.User;
using Ehrms.Authentication.API.Handlers.User.Commands;

namespace Ehrms.Authentication.API.IntegrationTests.ControllerTests.UserController;

public class UserControllerPostIntegrationTests : AuthenticationApiBaseIntegrationTest
{
    public UserControllerPostIntegrationTests(AuthenticationWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Update_InvalidUserId_ReturnsBadRequest()
    {
        var command = new UpdateUserCommandFaker().Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.Update, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_NoUserRoles_ReturnsBadRequest()
    {
        var command = new UpdateUserCommandFaker()
            .WithRoles(Enumerable.Empty<UserRole>())
            .Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.Update, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_ExistingUserId_ReturnsOk()
    {
        var user = new UserFaker()
            .WithAccountStatus(false)
            .Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var command = new UpdateUserCommandFaker()
            .WithRoles([UserRole.Admin, UserRole.User])
            .WithId(user.Id)
            .Generate();

        var response = await client.PostAsJsonAsync(UserControllerEndpoints.Update, command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var readUserDto = await response.Content.ReadFromJsonAsync<UserUpdateResponseDto>();

        readUserDto.Should().BeEquivalentTo(command, opt =>
            opt.Excluding(p => p.Roles));
    }
}