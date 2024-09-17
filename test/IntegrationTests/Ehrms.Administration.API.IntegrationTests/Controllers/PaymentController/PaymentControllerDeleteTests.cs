using Ehrms.Administration.TestHelpers.Fakers;
using Ehrms.Administration.TestHelpers.Fakers.Employee;

namespace Ehrms.Administration.API.IntegrationTests.Controllers.PaymentController;
public class PaymentControllerDeleteTests : AdministrationApiBaseIntegrationTest
{
	public PaymentControllerDeleteTests(AdministrationWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Delete_ExistingPaymentRecordId_DeletesRecordAndReturnsNoContent()
	{
		var employee = new EmployeeFaker().Generate();
		var paymentCategory = new PaymentCategoryFaker().Generate();
		var payment = new PaymentCriteriaFaker()
			.WithEmployee(employee)
			.WithPaymentCategory(paymentCategory)
			.Generate();
		await dbContext.AddAsync(payment);
		await dbContext.SaveChangesAsync();

		var response = await client.DeleteAsync($"{Endpoints.Payment}/{payment.Id}");
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Delete_NonExistingPaymentRecordId_ReturnsNotFound()
	{
		var response = await client.DeleteAsync($"{Endpoints.Payment}/{Guid.NewGuid()}");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Delete_InvalidPaymentRecordId_ReturnsBadRequest()
	{
		var response = await client.DeleteAsync($"{Endpoints.Payment}/{Guid.Empty}");
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}
