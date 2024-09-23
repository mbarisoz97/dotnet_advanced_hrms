using Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public class SkillControllerTests : BaseEmployeeInfoIntegrationTest
{
	public SkillControllerTests(EmployeeInfoWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Put_ValidSkillName_ReturnsOkWithReadSkillDto()
	{
		var createSkillCommand = new CreateSkillCommandFaker().Generate();

		var response = await client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillCommand);
		var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		createSkillResponse?.Id.Should().NotBe(Guid.Empty);
		createSkillResponse?.Name.Should().Be(createSkillCommand.Name);
	}

	[Fact]
	public async Task Get_ExistingSkillId_ReturnsOkWithReadSkillDto()
	{
		var createSkillCommand = new CreateSkillCommandFaker().Generate();

		var response = await client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillCommand);
		response.EnsureSuccessStatusCode();
		var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

		response = await client.GetAsync($"{Endpoints.EmployeeSkillsApi}/{createSkillResponse?.Id}");
		var readSkillDto = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		readSkillDto?.Id.Should().Be(createSkillResponse!.Id);
		readSkillDto?.Name.Should().Be(createSkillResponse!.Name);
	}

	[Fact]
	public async Task Get_NonExistingSkillId_ReturnsNotFound()
	{
		var response = await client.GetAsync($"{Endpoints.EmployeeSkillsApi}/{Guid.NewGuid()}");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Delete_ExistingSkillId_ReturnsNoContent()
	{
		var createSkillCommand = new CreateSkillCommandFaker().Generate();

		var response = await client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillCommand);
		response.EnsureSuccessStatusCode();
		var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadTitleDto>();
		response = await client.DeleteAsync($"{Endpoints.EmployeeSkillsApi}/{createSkillResponse?.Id}");

		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Delete_NonExistingSkillId_ReturnsNotFound()
	{
		var response = await client.DeleteAsync($"{Endpoints.EmployeeSkillsApi}/{Guid.NewGuid()}");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Post_NonExistingSkillId_ReturnsNotFound()
	{
		var updateSkillCommand = new UpdateSkillCommandFaker().Generate();
		var response = await client.PostAsJsonAsync(Endpoints.EmployeeSkillsApi, updateSkillCommand);
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}