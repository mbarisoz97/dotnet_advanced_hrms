﻿using Ehrms.ProjectManagement.API.Database.Models;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using System.Net;

namespace Ehrms.ProjectManagement.API.IntegrationTests.Controllers.ProjectController;

public class ProjectControllerPostIntegrationTests : ProjectManagementApiBaseIntegrationTests
{
    public ProjectControllerPostIntegrationTests(ProjectManagementWebApplicationFactory factory) : base(factory)
    {
    }

	[Fact]
	public async Task Post_NonExistingProject_ReturnsNotFound()
	{
		var command = new UpdateProjectCommandFaker().Generate();
		var postResponse = await client.PostAsJsonAsync(Endpoints.ProjectApi, command);

		postResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Post_ValidUpdateDetails_ReturnsOkWithReadProjectDto()
	{
		var createProjectCommand = new CreateProjectCommandFaker().Generate();

		var putProjectResponse = await client.PutAsJsonAsync(Endpoints.ProjectApi, createProjectCommand);
		putProjectResponse.EnsureSuccessStatusCode();
		var readProjectDtoFromPut = await putProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		var updateProjectCommand = new UpdateProjectCommandFaker()
			.WithId(readProjectDtoFromPut!.Id)
			.Generate();

		var getProjectResponse = await client.PostAsJsonAsync($"{Endpoints.ProjectApi}", updateProjectCommand);
		var readProjectDtoFromPost = await getProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		getProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		readProjectDtoFromPost?.Should().BeEquivalentTo(updateProjectCommand,
			options => options.ExcludingMissingMembers());
	}

	[Fact]
	public async Task Post_EmploymentEnded_ReturnsOkWithReadProjectDto()
	{
		var employee = new EmployeeFaker().Generate();
		await dbContext.AddAsync(employee);
		
		var project = new ProjectFaker().Generate();
		await dbContext.AddAsync(project);

		var employment = new EmploymentFaker()
			.WithProject(project)
			.WithEmployee(employee)
			.Generate();

		await dbContext.AddAsync(employment);
		await dbContext.SaveChangesAsync();

		var updateProjectCommand = new UpdateProjectCommandFaker()
			.WithProject(project)
			.WithEmployees(Array.Empty<Employee>())
			.Generate();

		var getProjectResponse = await client.PostAsJsonAsync($"{Endpoints.ProjectApi}", updateProjectCommand);
		var readProjectDtoFromPost = await getProjectResponse.Content.ReadFromJsonAsync<ReadProjectDto>();

		getProjectResponse.StatusCode.Should().Be(HttpStatusCode.OK);

		readProjectDtoFromPost?.Employees.Should().HaveCount(updateProjectCommand.Employees.Count);
		readProjectDtoFromPost?.Should().BeEquivalentTo(updateProjectCommand, 
			options => options.ExcludingMissingMembers());
	}

	[Fact]
	public async Task Post_EmptyProjectId_ReturnsBadRequest()
	{
		var command = new UpdateProjectCommandFaker()
			.WithId(Guid.Empty)
			.Generate();
		var postResponse = await client.PostAsJsonAsync(Endpoints.ProjectApi, command);

		postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Post_ShortProjetName_ReturnsBadRequest()
	{
		var command = new UpdateProjectCommandFaker().Generate();
		command.Name = "s";
		var postResponse = await client.PostAsJsonAsync(Endpoints.ProjectApi, command);

		postResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}
