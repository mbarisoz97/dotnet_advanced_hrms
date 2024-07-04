using Ehrms.Administration.API.Context;
using MediatR;

namespace Ehrms.Administration.API.Handlers.PaymentCategory.Queries;

public sealed record GetAllPaymentCategoriesQuery : IRequest<IQueryable<Models.PaymentCategory>> { }

internal sealed class GetAllPaymentCategoriesQueryHandler : IRequestHandler<GetAllPaymentCategoriesQuery, IQueryable<Models.PaymentCategory>>
{
	private readonly AdministrationDbContext _dbContext;

	public GetAllPaymentCategoriesQueryHandler(AdministrationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IQueryable<Models.PaymentCategory>> Handle(GetAllPaymentCategoriesQuery request, CancellationToken cancellationToken)
	{
		return await Task.FromResult(_dbContext.PaymentCategories
			.AsNoTracking()
			.AsQueryable());
	}
}