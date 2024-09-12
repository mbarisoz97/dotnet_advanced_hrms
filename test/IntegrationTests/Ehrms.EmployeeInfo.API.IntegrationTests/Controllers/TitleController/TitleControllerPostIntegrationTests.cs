using Ehrms.EmployeeInfo.API.Dtos.Title;
using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.TitleController;

public class TitleControllerPostIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public TitleControllerPostIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Post_ValidUpdateCommand_ReturnsOkWithTitleDto()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        await dbContext.SaveChangesAsync();

        var updateTitleCommand = new UpdateTitleCommandFaker()
            .WithId(title.Id)
            .Generate();
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeTitleApi, updateTitleCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var readTitleDto = await response.Content.ReadFromJsonAsync<ReadTitleDto>();

        readTitleDto?.Should().BeEquivalentTo(updateTitleCommand);
    }

    [Fact]
    public async Task Put_ShortTitleName_ReturnsBadRequest()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        await dbContext.SaveChangesAsync();

        var updateTitleCommand = new UpdateTitleCommandFaker()
            .WithId(title.Id)
            .WithName(new string('n', Consts.MinTitleNameLength - 1))
            .Generate();
        
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeTitleApi, updateTitleCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Put_LongTitleName_ReturnsBadRequest()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        await dbContext.SaveChangesAsync();

        var updateTitleCommand = new UpdateTitleCommandFaker()
            .WithId(title.Id)
            .WithName(new string('n', Consts.MaxTitleNameLength + 1))
            .Generate();
        
        var response = await client.PostAsJsonAsync(Endpoints.EmployeeTitleApi, updateTitleCommand);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}