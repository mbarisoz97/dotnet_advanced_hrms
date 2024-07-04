using Ehrms.Administration.API.Context;
using Ehrms.Administration.API.Exceptions;
using MediatR;

namespace Ehrms.Administration.API.Handlers.Payment.Commands;

public sealed class DeletePaymentCommand : IRequest<Guid>
{
	public Guid Id { get; set; }
}

internal sealed class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand, Guid>
{
	private readonly AdministrationDbContext _dbContext;

	public DeletePaymentCommandHandler(AdministrationDbContext administrationDbContext)
	{
		_dbContext = administrationDbContext;
	}

	public async Task<Guid> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
	{
		var paymentRecord = await _dbContext.PaymentCriteria
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
			?? throw new PaymentCriteriaNotFoundException($"Could not find payment record with id <{request.Id}>");

		_dbContext.Remove(paymentRecord);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return paymentRecord.Id;
	}
}
