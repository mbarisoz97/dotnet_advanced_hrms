using Ehrms.EmployeeInfo.API.Dtos.Title;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Command;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.TitleController;

public class TitleControllerPutIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public TitleControllerPutIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Put_ValidCreateCommand_ReturnsOkWithTitleDto()
    {
        var createTitleCommand = new CreatTitleCommandFaker().Generate();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeTitleApi, createTitleCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var readTitleDto = await response.Content.ReadFromJsonAsync<ReadTitleDto>();

        readTitleDto?.Id.Should().NotBe(Guid.Empty);
        readTitleDto?.TitleName.Should().Be(createTitleCommand.TitleName);
    }

    [Fact]
    public async Task Put_ShortTitleName_ReturnsBadRequest()
    {
        var createTitleCommand = new CreatTitleCommandFaker()
            .WithName(new string('n', Consts.MinTitleNameLength - 1))
            .Generate();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeTitleApi, createTitleCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_LongTitleName_ReturnsBadRequest()
    {
        var createTitleCommand = new CreatTitleCommandFaker()
            .WithName(new string('n', Consts.MaxTitleNameLength + 1))
            .Generate();
        var response = await client.PutAsJsonAsync(Endpoints.EmployeeTitleApi, createTitleCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}