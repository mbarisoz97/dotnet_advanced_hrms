using Ehrms.Contracts.Project;

namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Project.Commands;

public class DeleteProjectCommandHandlerTests
{
	private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
	private readonly DeleteProjectCommandHandler _handler;
    private readonly ProjectDbContext _projectDbContext;

    public DeleteProjectCommandHandlerTests()
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProjectMappingProfile());
        }));

        _projectDbContext = new(new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        _handler = new(_projectDbContext, _publishEndpointMock.Object, mapper);
    }

    [Fact]
    public async Task Handle_ExistingProjectId_RemovesProject()
    {
        var project = new ProjectFaker().Generate();
        _projectDbContext.Projects.Add(project);
        await _projectDbContext.SaveChangesAsync();

        var command = new DeleteProjectCommand { Id = project.Id };
        await _handler.Handle(command, default);

        _projectDbContext.Projects
            .Should()
            .NotContain(project);
    }

    [Fact]
    public async Task Handle_SuccessfullyDeletedProject_PublishesProjectDeletedEvent()
    {
        ProjectDeletedEvent? projectDeletedEvent = null;
        _publishEndpointMock.Setup(x => x.Publish(It.IsAny<ProjectDeletedEvent>(), It.IsAny<CancellationToken>()))
            .Callback((ProjectDeletedEvent @event, CancellationToken cancellationToken) =>
            {
                projectDeletedEvent = @event;
            });

		var project = new ProjectFaker().Generate();
		_projectDbContext.Projects.Add(project);
		await _projectDbContext.SaveChangesAsync();

		var command = new DeleteProjectCommand { Id = project.Id };
		await _handler.Handle(command, default);

		_publishEndpointMock.Verify(x => x.Publish(It.IsAny<ProjectDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        projectDeletedEvent.Should().BeEquivalentTo(command);
	}

    [Fact]
    public async Task Handle_NonExistingProjectId_ThrowProjectNotFoundException()
    {
        await Assert.ThrowsAsync<ProjectNotFoundException>(() =>
        {
            return _handler.Handle(new DeleteProjectCommand(), default);
        });
    }
}