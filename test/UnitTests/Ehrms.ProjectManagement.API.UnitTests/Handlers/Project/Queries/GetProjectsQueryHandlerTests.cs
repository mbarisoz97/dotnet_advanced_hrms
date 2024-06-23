namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Project.Queries;

public class GetProjectsQueryHandlerTests
{
    private readonly GetProjectsQueryHandler _handler;
    private readonly ProjectDbContext _projectDbContext;

    public GetProjectsQueryHandlerTests()
    {
        _projectDbContext = new(new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        _handler = new(_projectDbContext);
    }

    [Fact]
    public async Task Handle_NoExistingProject_ReturnsEmptyCollection()
    {
        var projects = await _handler.Handle(new GetProjectsQuery(), default);
        projects.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ContextWithProjects_ReturnsAllProjects()
    {
        var projectFaker = new ProjectFaker();
        await _projectDbContext.Projects.AddAsync(projectFaker.Generate());
        await _projectDbContext.Projects.AddAsync(projectFaker.Generate());
        await _projectDbContext.SaveChangesAsync();

        var projects = await _handler.Handle(new GetProjectsQuery(), default);
        projects.Should().HaveCount(2);
    }
}