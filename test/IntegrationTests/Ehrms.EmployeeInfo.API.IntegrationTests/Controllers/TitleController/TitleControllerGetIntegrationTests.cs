using Ehrms.EmployeeInfo.API.Dtos.Title;
using Ehrms.EmployeeInfo.TestHelpers.Faker.Title.Model;

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
        returnedTitleCollection.Should().BeEquivalentTo(expectedTitleCollection,
          options => options
            .ComparingByMembers<Database.Models.Title>()
            .Excluding(x => x.Employees));
    }
}