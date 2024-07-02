using MediatR;
using Ehrms.Administration.API.Context;
using Ehrms.Administration.API.Exceptions;
using AutoMapper;

namespace Ehrms.Administration.API.Handlers.Payment.Commands;

public class CreatePaymentCategoryCommand : IRequest<Guid>
{
	public string Name { get; set; } = string.Empty;
}

internal sealed class CreatePaymentCategoryCommandHandler : IRequestHandler<CreatePaymentCategoryCommand, Guid>
{
	private readonly IMapper _mapper;
	private readonly AdministrationDbContext _dbContext;

	public CreatePaymentCategoryCommandHandler(IMapper mapper,AdministrationDbContext dbContext)
	{
		_mapper = mapper;
		_dbContext = dbContext;
	}

	public async Task<Guid> Handle(CreatePaymentCategoryCommand request, CancellationToken cancellationToken)
	{
		if (IsCategoryNameInUse(request.Name))
		{
			throw new CategoryNameAlreadyInUseException($"Category name <{request.Name}> is already in use");
		}

		var paymentCategory = _mapper.Map<Models.PaymentCategory>(request);

		await _dbContext.AddAsync(paymentCategory, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return paymentCategory.Id;
	}

	private bool IsCategoryNameInUse(string categoryName)
	{
		return _dbContext.PaymentCategories
			.Select(x => x.Name.Trim())
			.Where(name => name == categoryName.Trim())
			.Any();
	}
}
