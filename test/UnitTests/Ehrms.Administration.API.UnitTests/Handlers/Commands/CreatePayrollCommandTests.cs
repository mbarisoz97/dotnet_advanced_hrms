using Ehrms.Administration.API.Context;
using Ehrms.Administration.API.Exceptions;
using Ehrms.Administration.API.Handlers.Payroll.Commands;
using Ehrms.Administration.API.UnitTests.TestHelpers.Fakers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Ehrms.Administration.API.UnitTests.Handlers.Commands;

public class CreatePayrollCommandTests
{
    private readonly AdministrationDbContext _dbContext;
    private readonly CreatePayrollCommandHandler _handler;

    public CreatePayrollCommandTests()
    {
        _dbContext = new(new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        _handler = new(_dbContext);
    }

    [Fact]
    public async Task Handle_ExpiredPaymentCriteria_ThrowsPaymentCriteriaNotFoundException()
    {
        var employee = new EmployeeFaker().Generate();
        var payment = new PaymentCriteriaFaker()
            .WithExpirationDate()
            .WithEmployee(employee)
            .Generate();

        await _dbContext.AddAsync(employee);
        await _dbContext.AddAsync(payment);
        await _dbContext.SaveChangesAsync();

        var command = new CreatePayrollCommandFaker().Generate();

        await Assert.ThrowsAsync<PaymentCriteriaNotFoundException>(async () =>
        {
            await _handler.Handle(command, default);
        });
    }

    [Fact]
    public async Task Handle_NonExistingEmployeeId_ThrowsPaymentCriteriaNotFoundException()
    {
        var payment = new PaymentCriteriaFaker()
            .WithExpirationDate()
            .Generate();
        await _dbContext.AddAsync(payment);
        await _dbContext.SaveChangesAsync();

        var command = new CreatePayrollCommandFaker().Generate();

        await Assert.ThrowsAsync<PaymentCriteriaNotFoundException>(async () =>
        {
            await _handler.Handle(command, default);
        });
    }

    [Fact]
    public async Task Handle_ValidEmployeeAndPaymentCriteria_CreatesNewPayroll()
    {
        var fakeEmployee = new EmployeeFaker().Generate();
        await _dbContext.Employees.AddAsync(fakeEmployee);

        var fakePaymentCriteria = new PaymentCriteriaFaker()
            .WithEmployee(fakeEmployee)
            .Generate();

        await _dbContext.PaymentCriteria.AddAsync(fakePaymentCriteria);
        await _dbContext.SaveChangesAsync();

        var command = new CreatePayrollCommand
        {
            EmployeeId = fakeEmployee.Id,
        };

        var payroll = await _handler.Handle(command, default);

        payroll.Should().NotBe(Guid.Empty);
    }
}