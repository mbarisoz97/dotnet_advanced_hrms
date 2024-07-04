using Ehrms.Administration.API.Dto.PaymentCategorty;

namespace Ehrms.Administration.API.IntegrationTests.Controllers.PaymentCategoryController;

public class PaymentCategoryControllerPostTests : AdministrationApiBaseIntegrationTest
{
	public PaymentCategoryControllerPostTests(AdministrationWebApplicationFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task Post_ValidUniqueCategoryName_ReturnsOkWithCategoryId()
	{
		var fakePaymentCategory = new PaymentCategoryFaker().Generate();
		await dbContext.AddAsync(fakePaymentCategory);
		await dbContext.SaveChangesAsync();

		var updateCategoryCommand = new UpdatePaymentCategoryCommandFaker()
			.WithId(fakePaymentCategory.Id)
			.Generate();

		var response = await client.PostAsJsonAsync(Endpoints.PaymentCategory, updateCategoryCommand);
		response.StatusCode.Should().Be(HttpStatusCode.OK);

		var readPaymentCategorty = await response.Content.ReadFromJsonAsync<ReadPaymentCategoryDto>();
		readPaymentCategorty.Should().BeEquivalentTo(updateCategoryCommand);
	}

	[Fact]
	public async Task Post_EmptyCategoryId_ReturnsBadRequest()
	{
		var updateCategoryCommand = new UpdatePaymentCategoryCommandFaker()
			.WithId(Guid.Empty)
			.Generate();

		var response = await client.PostAsJsonAsync(Endpoints.PaymentCategory, updateCategoryCommand);
		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Post_NonExistingCategoryId_ReturnsNotFound()
	{
		var updateCategoryCommand = new UpdatePaymentCategoryCommandFaker()
			.WithId(Guid.NewGuid())
			.Generate();

		var response = await client.PostAsJsonAsync(Endpoints.PaymentCategory, updateCategoryCommand);
		response.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}