namespace Ehrms.Administration.API.IntegrationTests.Controllers.PayrollController;

public class PayrollControllerPutIntegrationTests : AdministrationApiBaseIntegrationTest
{
    public PayrollControllerPutIntegrationTests(AdministrationWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Put_ExistingEmployeeId_ReturnsOkWithPayrollId()
    {
        var employee = new EmployeeFaker().Generate();
        var paymentCriteria = new PaymentCriteriaFaker()
            .WithEmployee(employee)
            .Generate();
        
        await dbContext.AddAsync(employee);
        await dbContext.AddAsync(paymentCriteria);
        await dbContext.SaveChangesAsync();

        var response = await client.PutAsJsonAsync(Endpoints.PayrollApi, new CreatePayrollCommand { EmployeeId = employee.Id});
        var payrollId = response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        payrollId.Should().NotBe(Guid.Empty);
    }
}