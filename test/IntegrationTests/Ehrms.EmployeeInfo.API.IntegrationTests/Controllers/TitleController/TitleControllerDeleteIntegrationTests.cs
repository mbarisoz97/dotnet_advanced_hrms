using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.TitleController;

public class TitleControllerDeleteIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public TitleControllerDeleteIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Delete_ExistingTitleId_ReturnsOk()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        await dbContext.SaveChangesAsync();

        var response = await client.DeleteAsync($"{Endpoints.EmployeeTitleApi}/{title.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_NonExistingTitleId_ReturnsBadRequest()
    {
        var response = await client.DeleteAsync($"{Endpoints.EmployeeTitleApi}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}