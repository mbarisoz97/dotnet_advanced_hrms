﻿using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.EmployeeInfo.API.Exceptions.Title;

namespace Ehrms.EmployeeInfo.API.Handlers.Title.Command;

public sealed class DeleteTitleCommand : IRequest<Result<Guid>>
{
    public Guid Id { get; set; }
}

internal sealed class DeleteTitleCommandHandler : IRequestHandler<DeleteTitleCommand, Result<Guid>>
{
    private readonly EmployeeInfoDbContext _dbContext;

    public DeleteTitleCommandHandler(EmployeeInfoDbContext dbContext)
    {
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
        
        return new Result<Guid>(request.Id);
    }
}