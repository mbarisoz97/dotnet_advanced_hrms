using MediatR;
using Ehrms.Administration.API.Exceptions;
using Ehrms.Administration.API.Database.Context;
using Ehrms.Administration.API.Database.Models;

namespace Ehrms.Administration.API.Handlers.Payment.Queries;
public sealed class GetPaymentQuery : IRequest<PaymentCriteria>
{
	public Guid Id { get; set; }
}

internal sealed class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, PaymentCriteria>
{
	private readonly AdministrationDbContext _dbContext;
	public GetPaymentQueryHandler(AdministrationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<PaymentCriteria> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
	{
		var paymentRecord = await _dbContext.PaymentCriteria
			.Include(x => x.Employee)
			.Include(x => x.PaymentCategory)
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
			?? throw new PaymentCriteriaNotFoundException($"Could not find payment record with id <{request.Id}>");

		return await Task.FromResult(paymentRecord);
	}
}