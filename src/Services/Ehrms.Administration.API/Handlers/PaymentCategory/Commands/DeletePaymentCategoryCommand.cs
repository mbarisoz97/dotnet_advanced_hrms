using MediatR;
using Ehrms.Administration.API.Context;
using Ehrms.Administration.API.Exceptions;

namespace Ehrms.Administration.API.Handlers.Payment.Commands;

public class DeletePaymentCategoryCommand : IRequest<Guid>
{
	public Guid Id { get; set; }
}

internal sealed class DeletePaymentCategoryCommandHandler : IRequestHandler<DeletePaymentCategoryCommand, Guid>
{
	private readonly AdministrationDbContext _dbContext;
	public DeletePaymentCategoryCommandHandler(AdministrationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Guid> Handle(DeletePaymentCategoryCommand request, CancellationToken cancellationToken)
	{
		var category = _dbContext.PaymentCategories
			.FirstOrDefault(x => x.Id == request.Id)
			?? throw new PaymentCategoryNotFoundException($"Could not find payment category with id <{request.Id}>");

		_dbContext.Remove(category);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return category.Id;	
	}
}