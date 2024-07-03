using MediatR;
using Ehrms.Administration.API.Context;
using Ehrms.Administration.API.Exceptions;

namespace Ehrms.Administration.API.Handlers.Payroll.Commands;

public class CreatePayrollCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; set; }
}

internal class CreatePayrollCommandHandler : IRequestHandler<CreatePayrollCommand, Guid>
{
    private readonly AdministrationDbContext _dbContext;

    public CreatePayrollCommandHandler(AdministrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreatePayrollCommand request, CancellationToken cancellationToken)
    {
        var paymentCriteria = _dbContext.PaymentCriteria
            .FirstOrDefault(x => x.Employee.Id == request.EmployeeId) 
            ?? throw new PaymentCriteriaNotFoundException($"Could not find payment criteria for employee with id <{request.EmployeeId}>");

        Models.Payroll payroll = new()
        {
            PaymentCriteria = paymentCriteria
        };

        await _dbContext.AddAsync(payroll, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return payroll.Id;
    }
}