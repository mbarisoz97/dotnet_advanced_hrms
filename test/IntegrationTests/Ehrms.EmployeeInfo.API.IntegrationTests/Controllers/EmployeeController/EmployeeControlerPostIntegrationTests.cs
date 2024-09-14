using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.EmployeeController;

public class EmployeeControlerPostIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public EmployeeControlerPostIntegrationTests(EmployeeInfoWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Post_ExistingEmployeeId_ReturnsOkWithUpdatedReadEmployeeDto()
    {
        var employee = new EmployeeFaker().Generate();
        await dbContext.AddAsync(employee);

        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        await dbContext.SaveChangesAsync();

        var updateEmployeeCommand = new UpdateEmployeeCommandFaker()
            .WithId(employee.Id)
            .WithTitleId(title.Id)
            .Generate();

        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, updateEmployeeCommand);
        response.EnsureSuccessStatusCode();
        var updateEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadTitleDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        updateEmployeeResponse.Should().BeEquivalentTo(updateEmployeeCommand,
            opt => opt.Excluding(p => p.Title));

        title.Should().BeEquivalentTo(updateEmployeeResponse?.Title);
    }

    [Fact]
    public async Task Post_NonExistingEmployeeId_ReturnsBadRequest()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptyShortFirstName_ReturnsBadRequest()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        command.FirstName = "s";
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptyOrShortLastName_ReturnsBadRequest()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        command.LastName = "s";
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_EmptyId_RetursBadRequest()
    {
        var command = new UpdateEmployeeCommandFaker().Generate();
        command.Id = Guid.Empty;
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeApi, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}