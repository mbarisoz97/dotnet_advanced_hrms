using Ehrms.Authentication.API.Dto.Role;

namespace Ehrms.Authentication.API.IntegrationTests.ControllerTests.UserRoleController;

public class UserRoleControllerGetTests : AuthenticationApiBaseIntegrationTest
{
    public UserRoleControllerGetTests(AuthenticationWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetAllUserRoles_ReturnsAllExistingRoles()
    {
        var response = await client.GetAsync(UserRoleControllerEndpoints.Base);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var userRoleDtos = await response.Content.ReadFromJsonAsync<IEnumerable<ReadUserRoleDto>>();
        userRoleDtos.Should().NotBeNull();
        userRoleDtos.Should().BeEquivalentTo(dbContext.Roles, opt => opt.ExcludingMissingMembers());
    }
}