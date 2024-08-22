using Ehrms.Authentication.API.Dto.User;
using Ehrms.Authentication.API.Handlers.User.Commands;

namespace Ehrms.Authentication.API.IntegrationTests.ControllerTests.UserController;

public class UserControllerGetIntegrationTests : AuthenticationApiBaseIntegrationTest
{
    public UserControllerGetIntegrationTests(AuthenticationWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetUsers_ReturnsOkWithAllUsers()
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
        var role = new RoleFaker().Generate();
        await dbContext.AddAsync(role);
        
        var user = new UserFaker().Generate();
        await dbContext.AddAsync(user);
        
        var userRole = new UserRoleFaker() 
            .WithUser(user)
            .WithRole(role)
            .Generate();

        await dbContext.AddAsync(userRole);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync($"{UserControllerEndpoints.Base}/{user.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var readUserDto = await response.Content.ReadFromJsonAsync<ReadUserDto>();
        readUserDto?.Username.Should().Be(user.UserName);
        readUserDto?.Email.Should().Be(user.Email);
        readUserDto?.IsActive.Should().Be(user.IsActive);
        readUserDto?.MustChangePassword.Should().Be(user.MustChangePassword);
        
        readUserDto?.Roles.Should().BeEquivalentTo(
            user.UserRoles.Select(x=>x.Role!.Name));
    }
    
    [Fact]
    public async Task GetUserById_UserWithNonAdminRole_ReturnsForbidden()
    {
        SetClientForUserWithRoles([UserRoles.User]);
        
        var response = await client.GetAsync($"{UserControllerEndpoints.Base}/{Guid.Empty}");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task GetUsers_UserWithNonAdminRole_ReturnsForbidden()
    {
        SetClientForUserWithRoles([UserRoles.User]);
        
        var response = await client.GetAsync(UserControllerEndpoints.Base);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}