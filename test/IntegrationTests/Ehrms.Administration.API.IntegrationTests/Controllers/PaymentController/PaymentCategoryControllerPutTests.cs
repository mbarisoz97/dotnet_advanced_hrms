using Ehrms.Administration.API.IntegrationTests.TestHelpers.Fakers.Payment;

namespace Ehrms.Administration.API.IntegrationTests.Controllers.PaymentController;

public class PaymentControllerPutTests : AdministrationApiBaseIntegrationTest
{
	public PaymentControllerPutTests(AdministrationWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Put_ValidPaymentCommand_ReturnsOkWithPaymentRecordId()
	{
		var employee = new EmployeeFaker().Generate();
		await dbContext.AddAsync(employee);
		var paymentCategory = new PaymentCategoryFaker().Generate();
		await dbContext.AddAsync(paymentCategory);
		await dbContext.SaveChangesAsync();

		var command = new CreatePaymentCommandFaker()
			.WithEmployeeId(employee.Id)
			.WithPaymentCategoryId(paymentCategory.Id)
			.Generate();

		var response = await client.PutAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var paymentId = await response.Content.ReadFromJsonAsync<Guid>();
		paymentId.Should().NotBe(Guid.Empty);
	}

	[Fact]
	public async Task Put_NonExistingEmployeeId_ReturnsNotFound()
	{
		var paymentCategory = new PaymentCategoryFaker().Generate();
		await dbContext.AddAsync(paymentCategory);
		await dbContext.SaveChangesAsync();

		var command = new CreatePaymentCommandFaker()
			.WithEmployeeId(Guid.NewGuid())
			.WithPaymentCategoryId(paymentCategory.Id)
			.Generate();

		var response = await client.PutAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Put_InvalidEmployeeId_ReturnsBadRequest()
	{
		var command = new CreatePaymentCommandFaker()
			.WithEmployeeId(Guid.Empty)
			.WithPaymentCategoryId(Guid.NewGuid())
			.Generate();

		var response = await client.PutAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Put_InvalidPaymentCategoryId_ReturnsBadRequest()
	{
		var command = new CreatePaymentCommandFaker()
			.WithEmployeeId(Guid.NewGuid())
			.WithPaymentCategoryId(Guid.Empty)
			.Generate();

		var response = await client.PutAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Put_InvalidPaymentAmount_ReturnsBadRequest()
	{
		var command = new CreatePaymentCommandFaker()
			.WithEmployeeId(Guid.NewGuid())
			.WithPaymentCategoryId(Guid.NewGuid())
			.WithAmount(0)
			.Generate();

		var response = await client.PutAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}