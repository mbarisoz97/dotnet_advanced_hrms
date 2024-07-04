namespace Ehrms.Administration.API.IntegrationTests.Controllers.PaymentCategoryController;

public class PaymentCategoryControllerDeleteTests : AdministrationApiBaseIntegrationTest
{
	public PaymentCategoryControllerDeleteTests(AdministrationWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Delete_ExistingCategory_ReturnsNoContent()
	{
		var fakeCategory = new PaymentCategoryFaker().Generate();
		await dbContext.AddAsync(fakeCategory);
		await dbContext.SaveChangesAsync();

		var response = await client.DeleteAsync($"{Endpoints.PaymentCategory}/{fakeCategory.Id}");
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Delete_InvalidCategoryId_ReturnsBadRequest()
	{
		var response = await client.DeleteAsync($"{Endpoints.PaymentCategory}/{Guid.Empty}");
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Delete_NonExistingCategory_ReturnsNotFound()
	{
		var response = await client.DeleteAsync($"{Endpoints.PaymentCategory}/{Guid.NewGuid()}");
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}