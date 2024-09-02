using Ehrms.Authentication.API.Database.Context;

namespace Ehrms.Authentication.API.PipelineBehaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ApplicationUserDbContext _dbContext;

    public TransactionBehavior(ApplicationUserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await next();
            await transaction.CommitAsync(cancellationToken);   
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}