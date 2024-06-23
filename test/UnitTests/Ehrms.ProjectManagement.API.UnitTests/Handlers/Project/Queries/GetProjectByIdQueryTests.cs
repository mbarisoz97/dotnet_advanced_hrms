namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Project.Queries;

public class GetProjectByIdQueryTests
{
    private readonly GetProjectByIdQueryHandler _handler;
    private readonly ProjectDbContext _projectDbContext;

    public GetProjectByIdQueryTests()
    {
        _projectDbContext = new(new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        _handler = new(_projectDbContext);
    }

    [Fact]
    public async Task Handle_NonExistingProjectId_ThrowProjectNotFoundException()
    {
        await Assert.ThrowsAsync<ProjectNotFoundException>(() =>
        {
            GetProjectByIdQuery query = new();
            return _handler.Handle(query, default);
        });
    }

    [Fact]
    public async Task Handle_ExistingProjectId_ReturnsQueriedProject()
    {
        var expectedProject = new ProjectFaker().Generate();
        await _projectDbContext.Projects.AddAsync(expectedProject);
        await _projectDbContext.SaveChangesAsync();

        GetProjectByIdQuery query = new() { Id = expectedProject.Id };
        var returnedProject = await _handler.Handle(query, default);

        returnedProject.Should()
            .Be(expectedProject);
    }
}