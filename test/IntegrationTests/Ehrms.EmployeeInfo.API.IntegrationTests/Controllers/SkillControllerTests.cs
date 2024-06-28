using Ehrms.Shared;
using System.Net.Http.Headers;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public class SkillControllerTests : IClassFixture<EmployeeInfoWebApplicationFactory>
{
	private readonly EmployeeInfoWebApplicationFactory _factory;
	private readonly HttpClient _client;

	public SkillControllerTests(EmployeeInfoWebApplicationFactory factory)
	{
		_factory = factory;
		_client = _factory.CreateClient();
		var request = new AuthenticationRequest
		{
			Username = "TestUser",
			Password = "TestPassword"
		};
		var jwt = new JwtTokenHandler().Generate(request);
		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.Token);
		_factory = factory;
	}

	[Fact]
	public async Task Put_ValidSkillName_ReturnsOkWithReadSkillDto()
	{
		var createSkillCommand = new CreateSkillCommandFaker().Generate();

		var response = await _client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillCommand);
		var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		createSkillResponse?.Id.Should().NotBe(Guid.Empty);
		createSkillResponse?.Name.Should().Be(createSkillCommand.Name);
	}

	[Fact]
	public async Task Get_ExistingSkillId_ReturnsOkWithReadSkillDto()
	{
		var createSkillCommand = new CreateSkillCommandFaker().Generate();

		var response = await _client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillCommand);
		response.EnsureSuccessStatusCode();
		var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

		response = await _client.GetAsync($"{Endpoints.EmployeeSkillsApi}/{createSkillResponse?.Id}");
		var readSkillDto = await response.Content.ReadFromJsonAsync<ReadSkillDto>();

		response.StatusCode.Should().Be(HttpStatusCode.OK);
		readSkillDto?.Id.Should().Be(createSkillResponse!.Id);
		readSkillDto?.Name.Should().Be(createSkillResponse!.Name);
	}

	[Fact]
	public async Task Get_NonExistingSkillId_ReturnsNotFound()
	{
		var response = await _client.GetAsync($"{Endpoints.EmployeeSkillsApi}/{Guid.NewGuid()}");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Delete_ExistingSkillId_ReturnsNoContent()
	{
		var createSkillCommand = new CreateSkillCommandFaker().Generate();

		var response = await _client.PutAsJsonAsync(Endpoints.EmployeeSkillsApi, createSkillCommand);
		response.EnsureSuccessStatusCode();
		var createSkillResponse = await response.Content.ReadFromJsonAsync<ReadEmployeeDto>();
		response = await _client.DeleteAsync($"{Endpoints.EmployeeSkillsApi}/{createSkillResponse?.Id}");

		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Delete_NonExistingSkillId_ReturnsNotFound()
	{
		var response = await _client.DeleteAsync($"{Endpoints.EmployeeSkillsApi}/{Guid.NewGuid()}");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Post_NonExistingSkillId_ReturnsNotFound()
	{
		var updateSkillCommand = new UpdateSkillCommandFaker().Generate();
		var response = await _client.PostAsJsonAsync(Endpoints.EmployeeSkillsApi, updateSkillCommand);
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}