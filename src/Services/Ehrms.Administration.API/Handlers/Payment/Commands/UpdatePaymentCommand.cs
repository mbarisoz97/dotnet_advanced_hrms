using MediatR;
using AutoMapper;
using Ehrms.Administration.API.Context;
using Ehrms.Administration.API.Exceptions;

namespace Ehrms.Administration.API.Handlers.Payment.Commands;

public class UpdatePaymentCommand : IRequest<PaymentCriteria>
{
	public Guid Id { get; set; }
	public decimal Amount { get; set; }
}

internal sealed class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, PaymentCriteria>
{
	private readonly IMapper _mapper;
	private readonly AdministrationDbContext _dbContext;

	public UpdatePaymentCommandHandler(IMapper mapper, AdministrationDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task<PaymentCriteria> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
	{
		var paymentRecord = await _dbContext.PaymentCriteria
			.Include(x=>x.Employee)
			.Include(x=>x.PaymentCategory)
			.FirstOrDefaultAsync(x => x.Id == request.Id)
			?? throw new PaymentCriteriaNotFoundException($"Could not find payment record with id <{request.Id}>");

		_mapper.Map(request, paymentRecord);

		_dbContext.Update(paymentRecord);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return paymentRecord;
	}
}