﻿using Ehrms.Contracts.Project;

namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Project.Commands;

public class UpdateProjectCommandHandlerTests
{
	private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
	private readonly UpdateProjectCommandHandler _handler;
	private readonly ProjectDbContext _projectDbContext;

	public UpdateProjectCommandHandlerTests()
	{
		IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfile(new ProjectMappingProfile());
		}));

		_projectDbContext = new(new DbContextOptionsBuilder()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options);

		_handler = new(mapper, _publishEndpointMock.Object, _projectDbContext);
	}

	[Fact]
	public async Task Handle_NonExistingProject_ThrowsProjectNotFoundException()
	{
		await Assert.ThrowsAsync<ProjectNotFoundException>(() =>
		{
			return _handler.Handle(new UpdateProjectCommand(), default);
		});
	}

	[Fact]
	public async Task Handle_ExistingProject_UpdateProjectDetails()
	{
		var project = new ProjectFaker().Generate();
		_projectDbContext.Projects.Add(project);
		await _projectDbContext.SaveChangesAsync();

		UpdateProjectCommand command = new UpdateProjectCommandFaker().Generate();
		command.Id = project.Id;

		var returnedProject = await _handler.Handle(command, default);

		returnedProject.Id.Should().Be(command.Id);
		returnedProject.Name.Should().Be(command.Name);
		returnedProject.Description.Should().Be(command.Description);
	}

	[Fact]
	public async Task Handle_NewEmploymentRecord_AddsNewRecordForEmployment()
	{
		var project = new ProjectFaker().Generate();
		await _projectDbContext.Projects.AddAsync(project);
		await _projectDbContext.SaveChangesAsync();

		var employee = new EmployeeFaker().Generate();
		await _projectDbContext.Employees.AddAsync(employee);
		await _projectDbContext.SaveChangesAsync();

		var command = new UpdateProjectCommandFaker().Generate();
		command.Id = project.Id;
		command.Employees.Add(employee.Id);

		var updatedProject = await _handler.Handle(command, default);

		updatedProject.Employments.Should().HaveCount(1);
		updatedProject.Employments.Should().OnlyContain(x => x.EndedAt == null);
	}

	[Fact]
	public async Task Handle_RemovedEmploymentRecord_UpdatesEndDateOfEmploymentRecord()
	{
		var employee = new EmployeeFaker().Generate();
		await _projectDbContext.Employees.AddAsync(employee);
		await _projectDbContext.SaveChangesAsync();

		var project = new ProjectFaker().Generate();
		await _projectDbContext.Projects.AddAsync(project);
		await _projectDbContext.SaveChangesAsync();

		var employment = new EmploymentFaker().Generate();
		employment.Project = project;
		employment.Employee = employee;
		await _projectDbContext.Employments.AddAsync(employment);
		await _projectDbContext.SaveChangesAsync();

		var command = new UpdateProjectCommandFaker().Generate();
		command.Id = project.Id;

		var updatedProject = await _handler.Handle(command, default);

		updatedProject.Employments.Should().HaveCount(1);
		updatedProject.Employments.Should().OnlyContain(x => x.EndedAt != null);
	}

	[Fact]
	public async Task Handle_UpdatedSkillRequirements_UpdatesProjectDetails()
	{
		var skills = new SkillFaker().Generate(4);
		await _projectDbContext.AddRangeAsync(skills);

		var project = new ProjectFaker()
			.WithRequiredSkills(skills)
			.Generate();
		await _projectDbContext.Projects.AddAsync(project);
		await _projectDbContext.SaveChangesAsync();

		var updatedSkillSet = skills.Take(2).ToList();
		var command = new UpdateProjectCommandFaker()
			.WithRequiredSkills(updatedSkillSet)
			.Generate();
		command.Id = project.Id;

		var updatedProject = await _handler.Handle(command, default);

		updatedProject.Should().BeEquivalentTo(command, options =>
			options.Excluding(x => x.RequiredSkills)
				   .Excluding(x => x.Employees));

		updatedProject.RequiredProjectSkills.Should().HaveCount(updatedSkillSet.Count);
	}

	[Fact]
	public async Task Handle_SuccessfullyUpdatedProject_PublishedProjectUpdatedEvent()
	{
		ProjectUpdatedEvent? projectUpdatedEvent = null;
		_publishEndpointMock.Setup(x => x.Publish(It.IsAny<ProjectUpdatedEvent>(), It.IsAny<CancellationToken>()))
			.Callback((ProjectUpdatedEvent @event, CancellationToken CancellationToken) =>
			{
				projectUpdatedEvent = @event;
			});

		var skills = new SkillFaker().Generate(2);
		await _projectDbContext.AddRangeAsync(skills);
		var employees = new EmployeeFaker().Generate(2);
		await _projectDbContext.AddRangeAsync(employees);

		var project = new ProjectFaker().Generate();
		await _projectDbContext.AddAsync(project);
		await _projectDbContext.SaveChangesAsync();

		var updateProjectCommand = new UpdateProjectCommandFaker()
				.WithId(project.Id)
				.WithEmployees(employees)
				.WithRequiredSkills(skills)
				.Generate();

		await _handler.Handle(updateProjectCommand, default);
		_publishEndpointMock.Verify(x => x.Publish(It.IsAny<ProjectUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
		projectUpdatedEvent.Should().BeEquivalentTo(updateProjectCommand, opts => opts.Excluding(x => x.Description));
	}
}