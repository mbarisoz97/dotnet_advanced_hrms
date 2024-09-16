using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.EmployeeController;

public class EmployeeControlerPutIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public EmployeeControlerPutIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Put_ValidEmployeeDetails_ReturnsOkWithEmployeeDto()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);    
        await dbContext.SaveChangesAsync(); 

        var command = new CreateEmployeeCommandFaker()
            .WithTitleId(title.Id)
            .Generate();

        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, command);
        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadTitleDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        createEmployeeResponse?.Id.Should().NotBe(Guid.Empty);
        createEmployeeResponse?.Should().BeEquivalentTo(command, opt => opt.Excluding(p => p.Title));
        title.Should().BeEquivalentTo(createEmployeeResponse?.Title);
    }

    [Fact]
    public async Task Put_ShortFirstName_ReturnsBadRequest()
    {
        var command = new CreateEmployeeCommandFaker().Generate();
        command.FirstName = "s";

        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_ShortLastNameName_ReturnsBadRequest()
    {
        var command = new CreateEmployeeCommandFaker().Generate();
        command.LastName = "t";

        var response = await client.PutAsJsonAsync(Endpoints.EmployeeApi, command);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}