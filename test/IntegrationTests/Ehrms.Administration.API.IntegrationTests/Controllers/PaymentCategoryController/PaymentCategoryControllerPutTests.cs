namespace Ehrms.Administration.API.IntegrationTests.Controllers.PaymentCategoryController;

public class PaymentCategoryControllerPutTests : AdministrationApiBaseIntegrationTest
{
	public PaymentCategoryControllerPutTests(AdministrationWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Put_ValidUniqueCategoryName_ReturnsOkWithCategoryId()
	{
		var command = new CreatePaymentCategoryCommandFaker().Generate();
		var response = await client.PutAsJsonAsync(Endpoints.PaymentCategory, command);

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task Put_CategoryNameInUse_ReturnsBadRequest()
	{
		var fakePaymentCategory = new PaymentCategoryFaker().Generate();
		await dbContext.AddAsync(fakePaymentCategory);
		await dbContext.SaveChangesAsync();

		var command = new CreatePaymentCategoryCommandFaker()
			.WithName(fakePaymentCategory.Name)
			.Generate();
		var response = await client.PutAsJsonAsync(Endpoints.PaymentCategory, command);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Put_EmptyCategoryName_ReturnsBadRequest()
	{
		var command = new CreatePaymentCategoryCommandFaker()
			.WithName(string.Empty)
			.Generate();
		var response = await client.PutAsJsonAsync(Endpoints.PaymentCategory, command);

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}