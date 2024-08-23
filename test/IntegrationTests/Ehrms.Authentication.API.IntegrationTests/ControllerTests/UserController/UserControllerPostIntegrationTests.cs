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
            .WithRoles(Enumerable.Empty<UserRoles>())
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
            .WithRoles([UserRoles.Admin, UserRoles.User])
            .WithId(user.Id)
            .Generate();

        var response = await client.PostAsJsonAsync(UserControllerEndpoints.Update, command);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var readUserDto = await response.Content.ReadFromJsonAsync<UserUpdateResponseDto>();
        readUserDto.Should().BeEquivalentTo(command, opt =>
            opt.Excluding(p => p.Roles));
    }

    [Fact]
    public async Task? Update_NonAdminUser_ReturnsForbidden()
    {
        SetClientForUserWithRoles([UserRoles.User]);
        var command = new UpdateUserCommandFaker().Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.Update, command);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task? ResetPassword_InactiveUser_ReturnsUnauthorized()
    {
        var user = new UserFaker()
            .WithUserName("InactiveTestUser")
            .WithAccountStatus(false)
            .Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var command = new UpdateUserPasswordCommandFaker()
            .WithUserName(user.UserName!)
            .Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task? ResetPassword_UnknownUser_ReturnsNotFound()
    {
        var user = new UserFaker().Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var command = new UpdateUserPasswordCommandFaker().Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #region ResetPassword

    [Fact]
    public async Task? ResetPassword_InactiveUserAccount_ReturnsUnauthorized()
    {
        var user = new UserFaker()
            .WithAccountStatus(false)
            .Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
        
        var command = new UpdateUserPasswordCommandFaker()
            .WithUserName(user.UserName!)
            .Generate();
        
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task? ResetPassword_WrongUserCredentials_ReturnsBadRequest()
    {
        var user = await AddRandomUser();
        var command = new UpdateUserPasswordCommandFaker()
            .WithUserName(user.UserName!)
            .Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ResetPassword_EmptyUsername_ReturnsBadRequest()
    {
        await AddRandomUser();
        var command = new UpdateUserPasswordCommandFaker()
            .WithUserName("")
            .Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ResetPassword_ShortCurrentPassword_ReturnsBadRequest()
    {
        await AddRandomUser();
        var command = new UpdateUserPasswordCommandFaker()
            .WithCurrentPassword(new string('s', Consts.MinUserNameLength - 1))
            .Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ResetPassword_LongCurrentPassword_ReturnsBadRequest()
    {
        await AddRandomUser();
        var command = new UpdateUserPasswordCommandFaker()
            .WithCurrentPassword(new string('s', Consts.MaxPasswordLength + 1))
            .Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ResetPassword_ShortNewPassword_ReturnsBadRequest()
    {
        await AddRandomUser();
        var command = new UpdateUserPasswordCommandFaker()
            .WithNewPassword(new string('s', Consts.MinPasswordLength - 1))
            .Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ResetPassword_LongNewPassword_ReturnsBadRequest()
    {
        await AddRandomUser();
        var command = new UpdateUserPasswordCommandFaker()
            .WithNewPassword(new string('s', Consts.MaxPasswordLength + 1))
            .Generate();
        var response = await client.PostAsJsonAsync(UserControllerEndpoints.PasswordReset, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion
}