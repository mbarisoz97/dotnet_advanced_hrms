using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.Handlers.Employee.Command;

public sealed class CreateEmployeeCommand : IRequest<Result<Database.Models.Employee>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public ReadTitleDto? Title { get; set; }
    public ICollection<Guid> Skills { get; set; } = [];
}

internal sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<Database.Models.Employee>>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly EmployeeInfoDbContext _dbContext;

    public CreateEmployeeCommandHandler(IPublishEndpoint publishEndpoint, IMapper mapper, EmployeeInfoDbContext dbContext)
    {
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<Database.Models.Employee>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        Database.Models.Employee employee = _mapper.Map<Database.Models.Employee>(request);

        await _dbContext.Skills
            .Where(x => request.Skills.Contains(x.Id))
            .ForEachAsync(employee.Skills.Add);

        if (request.Title == null)
        {
            return new Result<Database.Models.Employee>(
                new TitleNotFoundException("Employee title was invalid."));
        }

        var title = await _dbContext.Titles.FirstOrDefaultAsync(x => x.Id == request.Title.Id, cancellationToken);
        if (title == null)
        {
            return new Result<Database.Models.Employee>(
                new TitleNotFoundException($"Could not find employee title with id : <{request.Title.Id}>"));
        }

        employee.Title = title;
        await _dbContext.Employees.AddAsync(employee, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var employeeCreatedEvent = _mapper.Map<EmployeeCreatedEvent>(employee);
        await _publishEndpoint.Publish(employeeCreatedEvent, cancellationToken);

        return employee;
    }
}