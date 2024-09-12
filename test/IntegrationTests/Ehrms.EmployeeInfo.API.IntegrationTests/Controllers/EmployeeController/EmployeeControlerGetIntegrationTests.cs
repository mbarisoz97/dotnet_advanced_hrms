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

        var createEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
        response = await client.GetAsync($"{Endpoints.EmployeeApi}/{createEmployeeResponse?.Id}");
        var readEmployeeResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        readEmployeeResponse?.Id.Should().NotBe(Guid.Empty);

        var expectedEmployeeSkills = employee.Skills.Select(s => s.Id);
        readEmployeeResponse.Should().BeEquivalentTo(employee, opt => opt
            // Exclude properties that are not directly comparable    
            .Excluding(p => p.Title)
            .Excluding(p => p.Skills)
            // Custom comparison for TitleId
            .Using<Guid>(ctx => ctx.Subject.Should().Be(title.Id))
            .When(info => info.Path == nameof(ReadEmployeeDto.TitleId))
            // Custom comparison for Skills
            .Using<ICollection<Guid>>(ctx => ctx.Subject.Should().BeEquivalentTo(expectedEmployeeSkills))
            .When(info => info.Path == nameof(ReadEmployeeDto.Skills)));
    }
}