using MediatR;
using AutoMapper;
using Ehrms.Administration.API.Exceptions;
using Ehrms.Administration.API.Database.Context;

namespace Ehrms.Administration.API.Handlers.PaymentCategory.Commands;

public class UpdatePaymentCategoryCommand : IRequest<Database.Models.PaymentCategory>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

internal class UpdatePaymentCategoryCommandHandler : IRequestHandler<UpdatePaymentCategoryCommand, Database.Models.PaymentCategory>
{
    private readonly IMapper _mapper;
    private readonly AdministrationDbContext _dbContext;

    public UpdatePaymentCategoryCommandHandler(IMapper mapper, AdministrationDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<Database.Models.PaymentCategory> Handle(UpdatePaymentCategoryCommand request, CancellationToken cancellationToken)
    {
        var paymentCategory = _dbContext.PaymentCategories
            .FirstOrDefault(x => x.Id == request.Id)
            ?? throw new PaymentCategoryNotFoundException($"Could not find payment category with id <{request.Name}>");

        _mapper.Map(request, paymentCategory);

        _dbContext.Update(paymentCategory);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return paymentCategory;
    }
}