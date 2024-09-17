using Ehrms.Administration.API.Database.Context;
using MediatR;

namespace Ehrms.Administration.API.Handlers.PaymentCategory.Queries;

public sealed record GetAllPaymentCategoriesQuery : IRequest<IQueryable<Database.Models.PaymentCategory>> { }

internal sealed class GetAllPaymentCategoriesQueryHandler : IRequestHandler<GetAllPaymentCategoriesQuery, IQueryable<Database.Models.PaymentCategory>>
{
	private readonly AdministrationDbContext _dbContext;

	public GetAllPaymentCategoriesQueryHandler(AdministrationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IQueryable<Database.Models.PaymentCategory>> Handle(GetAllPaymentCategoriesQuery request, CancellationToken cancellationToken)
	{
		return await Task.FromResult(_dbContext.PaymentCategories
			.AsNoTracking()
			.AsQueryable());
	}
}