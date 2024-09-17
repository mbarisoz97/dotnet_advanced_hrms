using MediatR;
using Ehrms.Administration.API.Database.Context;
using Ehrms.Administration.API.Database.Models;

namespace Ehrms.Administration.API.Handlers.Payment.Queries;

public sealed record GetAllPaymentsQuery : IRequest<IQueryable<PaymentCriteria>> { };

internal sealed class GetAllPaymentsQueryHandler : IRequestHandler<GetAllPaymentsQuery, IQueryable<PaymentCriteria>>
{
	private readonly AdministrationDbContext _dbContext;

	public GetAllPaymentsQueryHandler(AdministrationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<IQueryable<PaymentCriteria>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
	{
		var paymentRecords = _dbContext.PaymentCriteria
			.Include(x => x.Employee)
			.Include(x => x.PaymentCategory)
			.AsNoTracking();

		return Task.FromResult(paymentRecords);
	}
}