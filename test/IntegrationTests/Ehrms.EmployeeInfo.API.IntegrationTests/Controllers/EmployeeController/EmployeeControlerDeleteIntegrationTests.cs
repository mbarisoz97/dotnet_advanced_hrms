using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.EmployeeController;

public class EmployeeControlerDeleteIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public EmployeeControlerDeleteIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Delete_ExistingEmployeeId_RemovesEmployee()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        await dbContext.SaveChangesAsync();

        var createEmployeeCommand = new CreateEmployeeCommandFaker()
            .WithTitleId(title.Id)
            .Generate();

        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, createEmployeeCommand);
        response.EnsureSuccessStatusCode();

        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadTitleDto>();
        response = await client.DeleteAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_NonExistingEmployeeId_ReturnsNotFound()
    {
        var response = await client.DeleteAsync($"{Endpoints.EmployeeApi}/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}
