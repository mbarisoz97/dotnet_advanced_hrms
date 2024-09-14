using Bogus.Extensions.UnitedKingdom;
using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;
using LanguageExt;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers.EmployeeController;

public class EmployeeControlerGetIntegrationTests : BaseEmployeeInfoIntegrationTest
{
    public EmployeeControlerGetIntegrationTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_ExistingEmployeeId_ReturnsOkWithEmployeeReadDto()
    {
        var title = new TitleFaker().Generate();
        await dbContext.AddAsync(title);
        var employee = new EmployeeFaker()
            .WithTitle(title)
            .Generate();
        await dbContext.AddAsync(employee);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync($"{Endpoints.EmployeeApi}/{employee.Id}");
        response.EnsureSuccessStatusCode();

        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadTitleDto>();
        response = await client.GetAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");
        var readEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadTitleDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readEmployeeResponse?.Id.Should().NotBe(Guid.Empty);

        readEmployeeResponse.Should().BeEquivalentTo(employee, opt => opt
            .Excluding(p => p.Skills)
            .Excluding(p => p.Title));

        readEmployeeResponse?.Skills.Should()
            .BeEquivalentTo(employee.Skills.Select(s => s.Id));

        readEmployeeResponse?.Title.Should()
            .BeEquivalentTo(title, opt => opt.ExcludingMissingMembers());
    }
}