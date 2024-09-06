using Ehrms.Contracts.Title;
using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.Handlers.Title.Command;

public sealed class UpdateTitleCommand : IRequest<Result<Database.Models.Title>>
{
    public Guid Id { get; set; }
    public string TitleName { get; set; } = string.Empty;
}

internal sealed class UpdateTitleCommandHandler : IRequestHandler<UpdateTitleCommand, Result<Database.Models.Title>>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly EmployeeInfoDbContext _dbContext;

    public UpdateTitleCommandHandler(IMapper mapper, IPublishEndpoint publishEndpoint, EmployeeInfoDbContext employeeInfoDbContext)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _dbContext = employeeInfoDbContext;
    }

    public async Task<Result<Database.Models.Title>> Handle(UpdateTitleCommand request, CancellationToken cancellationToken)
    {
        var title = await _dbContext.Titles.FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);
        if (title == null)
        {
            return new Result<Database.Models.Title>(
                new TitleNotFoundException($"Could not find title with id : {request.Id}"));
        }

        title = _mapper.Map(request, title);
        _dbContext.Update(title);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var titleUpdatedEvent = _mapper.Map<TitleUpdatedEvent>(title);
        await _publishEndpoint.Publish(titleUpdatedEvent, cancellationToken);

        return new Result<Database.Models.Title>(title);
    }
}