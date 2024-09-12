using Ehrms.EmployeeInfo.API.Dtos.Title;
using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.TitleController;

public class TitleControllerGetIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public TitleControllerGetIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_ReturnOkWithAllExistingTitles()
    {
        var expectedTitleCollection = new TitleFaker().Generate(3);
        await dbContext.AddRangeAsync(expectedTitleCollection);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync(Endpoints.EmployeeTitleApi);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var returnedTitleCollection = await response.Content.ReadFromJsonAsync<IEnumerable<ReadTitleDto>>();
        foreach (var model in expectedTitleCollection)
        {
            returnedTitleCollection.Should().ContainEquivalentOf(model,
             options => options.ComparingByMembers<Database.Models.Title>()
                               .ExcludingMissingMembers());
        }
    }

    [Fact]
    public async Task Get_ExistingTitleId_ReturnsOkWithReadTitleDto()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync($"{Endpoints.EmployeeTitleApi}/{title.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var returnedTitleDto = await response.Content.ReadFromJsonAsync<ReadTitleDto>();
        returnedTitleDto.Should().BeEquivalentTo(title, options => options.Excluding(x => x.Employees));
    }

    [Fact]
    public async Task Get_NonExistingTitleId_ReturnsBadRequest()
    {
        var response = await client.GetAsync($"{Endpoints.EmployeeTitleApi}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}