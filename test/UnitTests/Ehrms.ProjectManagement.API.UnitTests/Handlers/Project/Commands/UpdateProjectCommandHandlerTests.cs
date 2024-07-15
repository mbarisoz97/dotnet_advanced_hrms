using Ehrms.ProjectManagement.API.Database.Context;

namespace Ehrms.ProjectManagement.API.UnitTests.Handlers.Project.Commands;

public class UpdateProjectCommandHandlerTests
{
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

        _handler = new(mapper, _projectDbContext);
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
        updatedProject.Employments.Should().OnlyContain(x=> x.EndedAt != null);
    }
}