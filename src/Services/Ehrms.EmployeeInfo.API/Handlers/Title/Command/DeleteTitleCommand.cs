using Ehrms.Contracts.Title;
using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.Handlers.Title.Command;

public sealed class DeleteTitleCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal sealed class DeleteTitleCommandHandler : IRequestHandler<DeleteTitleCommand, Result<Guid>>
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly EmployeeInfoDbContext _dbContext;

    public DeleteTitleCommandHandler(IMapper mapper, IPublishEndpoint publishEndpoint, EmployeeInfoDbContext dbContext)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> Handle(DeleteTitleCommand request, CancellationToken cancellationToken)
    {
        var title = await _dbContext.Titles.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (title == null)
        {
            return new Result<Guid>(
                new TitleNotFoundException($"Could not find title with id <{request.Id}>"));
        }

        _dbContext.Remove(title);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        var titleDeletedEvent = _mapper.Map<TitleDeletedEvent>(request);
        await _publishEndpoint.Publish(titleDeletedEvent, default);

        return new Result<Guid>(request.Id);
    }
}