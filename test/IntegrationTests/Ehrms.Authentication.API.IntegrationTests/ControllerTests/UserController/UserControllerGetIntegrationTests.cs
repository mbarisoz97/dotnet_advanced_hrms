using LanguageExt;

namespace Ehrms.Authentication.API.IntegrationTests.ControllerTests.UserController;

public class UserControllerGetIntegrationTests : AuthenticationApiBaseIntegrationTest
{
    public UserControllerGetIntegrationTests(AuthenticationWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetUsers_ReturnsAllUsers()
    {
        var users = new UserFaker().Generate(10);
        await dbContext.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync(UserControllerEndpoints.Base);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userDtos = await response.Content.ReadFromJsonAsync<IEnumerable<ReadUserDto>>();
        userDtos?.Count().Should().Be(users.Count);
    }

    [Fact]
    public async Task GetUserById_NonExistingUserId_ReturnsBadRequest()
    {
        var response = await client.GetAsync($"{UserControllerEndpoints.Base}/{Guid.Empty}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetUserById_ExistingUserId_ReturnsOkWithUserDetails()
    {
        var roles = dbContext.Roles;
        var user = new UserFaker()
            .WithRoles([.. roles])
            .Generate();
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync($"{UserControllerEndpoints.Base}/{user.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var readUserDto = await response.Content.ReadFromJsonAsync<ReadUserDto>();
        readUserDto?.Username.Should().Be(user.UserName);
        readUserDto?.Email.Should().Be(user.Email);
        readUserDto?.IsActive.Should().Be(user.IsActive);
        readUserDto?.Roles.Should().BeEquivalentTo(
            user.Roles.Select(x=>x.Name));
    }
}