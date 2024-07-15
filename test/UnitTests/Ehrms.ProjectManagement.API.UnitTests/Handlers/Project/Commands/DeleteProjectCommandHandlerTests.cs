using Ehrms.ProjectManagement.API.Database.Context;
using FluentAssertions;

namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Project.Commands;

public class DeleteProjectCommandHandlerTests
{
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

        _handler = new(_projectDbContext);
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
    public async Task Handle_NonExistingProjectId_ThrowProjectNotFoundException()
    {
        await Assert.ThrowsAsync<ProjectNotFoundException>(() =>
        {
            return _handler.Handle(new DeleteProjectCommand(), default);
        });
    }
}