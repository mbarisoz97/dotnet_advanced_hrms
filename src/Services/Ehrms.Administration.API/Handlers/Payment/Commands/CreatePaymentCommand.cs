using MediatR;
using AutoMapper;
using Ehrms.Administration.API.Context;
using Ehrms.Administration.API.Exceptions;

namespace Ehrms.Administration.API.Handlers.Payment.Commands;

public class CreatePaymentCommand : IRequest<Guid>
{
	public decimal Amount { get; set; }
	public Guid EmployeeId { get; set; }
	public Guid PaymentCategoryId { get; set; }
}

internal sealed class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Guid>
{
	private readonly IMapper _mapper;
	private readonly AdministrationDbContext _dbContext;

	public CreatePaymentCommandHandler(IMapper mapper, AdministrationDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task<Guid> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
	{
		var employee = await _dbContext.Employees
			.FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
			?? throw new EmployeeNotFoundException($"Could not find employee with id <{request.EmployeeId}>");

		var paymentCategory = await _dbContext.PaymentCategories
			.FirstOrDefaultAsync(x => x.Id == request.PaymentCategoryId, cancellationToken)
			?? throw new PaymentCategoryNotFoundException($"Could not find payment category with id <{request.PaymentCategoryId}>");

		PaymentCriteria criteria = new()
		{
			Employee = employee,
			PaymentCategory = paymentCategory,
			CreatedAt = DateTime.UtcNow
		};

		_mapper.Map(request, criteria);

		await _dbContext.AddAsync(criteria, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return request.EmployeeId;
	}
}