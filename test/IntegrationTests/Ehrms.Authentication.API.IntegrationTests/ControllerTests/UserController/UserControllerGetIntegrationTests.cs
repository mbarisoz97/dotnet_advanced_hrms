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
}