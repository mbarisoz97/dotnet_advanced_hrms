using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.Handlers.Title.Command;

public sealed class CreateTitleCommand : IRequest<Result<Database.Models.Title>>
{
    public string TitleName { get; set; } = string.Empty;
}

internal sealed class CreateTitleCommandHandler : IRequestHandler<CreateTitleCommand, Result<Database.Models.Title>>
{
    private readonly IMapper _mapper;
    private readonly EmployeeInfoDbContext _dbContext;

    public CreateTitleCommandHandler(IMapper mapper, EmployeeInfoDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Result<Database.Models.Title>> Handle(CreateTitleCommand request, CancellationToken cancellationToken)
    {
        var existingTitle = await _dbContext.Titles.FirstOrDefaultAsync(cancellationToken);
        if (existingTitle != null)
        {
            return new Result<Database.Models.Title>(
                new TitleNameInUseException($"There is already a title with name {request.TitleName}"));
        }

        var newTitle = _mapper.Map<Database.Models.Title>(request);
        await _dbContext.AddAsync(newTitle, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Result<Database.Models.Title>(newTitle);
    }
}