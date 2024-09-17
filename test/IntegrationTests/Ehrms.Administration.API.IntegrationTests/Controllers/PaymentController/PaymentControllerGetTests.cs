namespace Ehrms.Administration.API.IntegrationTests.Controllers.PaymentController;

public class PaymentControllerGetTests : AdministrationApiBaseIntegrationTest
{
	public PaymentControllerGetTests(AdministrationWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Get_ExistingPaymentId_ReturnsOkWithReadPaymentDto()
	{
		var employee = new EmployeeFaker().Generate();
		var paymentCategory = new PaymentCategoryFaker().Generate();
		var payment = new PaymentCriteriaFaker()
			.WithEmployee(employee)
			.WithPaymentCategory(paymentCategory)
			.Generate();
		await dbContext.AddAsync(payment);
		await dbContext.SaveChangesAsync();

		var response = await client.GetAsync($"{Endpoints.Payment}/{payment.Id}");
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var readPaymentDto = await response.Content.ReadFromJsonAsync<ReadPaymentDto>();
		readPaymentDto?.Id.Should().Be(payment.Id);
		readPaymentDto?.Amount.Should().Be(payment.Amount);
		readPaymentDto?.EmployeeId.Should().Be(payment!.Employee!.Id);
		readPaymentDto?.PaymentCategoryId.Should().Be(payment!.PaymentCategory!.Id);
	}

	[Fact]
	public async Task Get_NonExistingPaymentId_ReturnsNotFound()
	{
		var response = await client.GetAsync($"{Endpoints.Payment}/{Guid.NewGuid()}");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Get_InvalidPaymentId_ReturnsBadRequest()
	{
		var response = await client.GetAsync($"{Endpoints.Payment}/{Guid.Empty}");
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Get_ReturnsAllPaymentRecords()
	{
		ClearPaymentRecords();

		var employee = new EmployeeFaker().Generate();
		var paymentCategory = new PaymentCategoryFaker().Generate();
		var paymentRecords = new PaymentCriteriaFaker()
			.WithEmployee(employee)
			.WithPaymentCategory(paymentCategory)
			.Generate(10);
		await dbContext.AddRangeAsync(paymentRecords);
		await dbContext.SaveChangesAsync();

		var response = await client.GetAsync($"{Endpoints.Payment}");
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var readPaymentDto = await response.Content.ReadFromJsonAsync<IEnumerable<ReadPaymentDto>>();
		readPaymentDto.Should().HaveCount(paymentRecords.Count);
	}

	private void ClearPaymentRecords()
	{
		dbContext.RemoveRange(dbContext.PaymentCriteria);
		dbContext.SaveChanges();
	}
}
