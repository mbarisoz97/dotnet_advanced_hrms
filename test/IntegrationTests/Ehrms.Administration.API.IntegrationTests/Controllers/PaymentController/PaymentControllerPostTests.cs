using Ehrms.Administration.API.Dto.Payment;
using Ehrms.Administration.API.IntegrationTests.TestHelpers.Fakers.Payment;

namespace Ehrms.Administration.API.IntegrationTests.Controllers.PaymentController;

public class PaymentControllerPostTests : AdministrationApiBaseIntegrationTest
{
	public PaymentControllerPostTests(AdministrationWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Post_ValidPaymentRecord_ReturnsOkWithReadPaymentDto()
	{
		var employee = new EmployeeFaker().Generate();
		var paymentCategory = new PaymentCategoryFaker().Generate();
		var payment = new PaymentCriteriaFaker()
			.WithEmployee(employee)
			.WithPaymentCategory(paymentCategory)
			.Generate();
		await dbContext.AddAsync(payment);
		await dbContext.SaveChangesAsync();

		var command = new UpdatePaymentCommandFaker()
			.WithId(payment.Id)
			.Generate();
		
		var response = await client.PostAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var readPaymentDto = await response.Content.ReadFromJsonAsync<ReadPaymentDto>();

		readPaymentDto?.Id.Should().Be(command.Id);
		readPaymentDto?.EmployeeId.Should().Be(payment.Employee.Id);
		readPaymentDto?.PaymentCategoryId.Should().Be(payment.PaymentCategory.Id);
		readPaymentDto.Should().BeEquivalentTo(command);
	}

	[Fact]
	public async Task Post_NonExistingPaymentId_ReturnsNotFound()
	{
		var command = new UpdatePaymentCommandFaker().Generate();

		var response = await client.PostAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Post_InvalidPaymentId_ReturnsBadRequest()
	{
		var command = new UpdatePaymentCommandFaker()
			.WithId(Guid.Empty)
			.Generate();

		var response = await client.PostAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Post_InvalidAmount_ReturnsBadRequest()
	{
		var command = new UpdatePaymentCommandFaker()
			.WithAmount(0)
			.Generate();

		var response = await client.PostAsJsonAsync(Endpoints.Payment, command);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}