using Ehrms.Administration.API.Context;
using Ehrms.Administration.API.Exceptions;
using MediatR;

namespace Ehrms.Administration.API.Handlers.PaymentCategory.Queries;

public class GetPaymentCategoryQuery : IRequest<Models.PaymentCategory>
{
	public Guid Id { get; set; }
}

internal sealed class GetPaymentCategoryCommandHandler : IRequestHandler<GetPaymentCategoryQuery, Models.PaymentCategory>
{
	private readonly AdministrationDbContext _dbContext;

	public GetPaymentCategoryCommandHandler(AdministrationDbContext dbContext)
    {
		_dbContext = dbContext;
	}

    public Task<Models.PaymentCategory> Handle(GetPaymentCategoryQuery request, CancellationToken cancellationToken)
	{
		var category = _dbContext.PaymentCategories
			.FirstOrDefault(x=>x.Id == request.Id)
			?? throw new PaymentCategoryNotFoundException($"Could not find payment category with id <{request.Id}>");

		return Task.FromResult(category);
	}
}
