using Ehrms.Administration.API.Dto.PaymentCategorty;

namespace Ehrms.Administration.API.IntegrationTests.Controllers.PaymentCategoryController;

public class PaymentCategoryControllerGetTests : AdministrationApiBaseIntegrationTest
{
    public PaymentCategoryControllerGetTests(AdministrationWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Get_ValidCategoryId_ReturnsOkWithCategoryDto()
    {
        var fakePaymentCategory = new PaymentCategoryFaker().Generate();
        await dbContext.AddAsync(fakePaymentCategory);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync($"{Endpoints.PaymentCategory}/{fakePaymentCategory.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var readCategoryDto = await response.Content.ReadFromJsonAsync<ReadPaymentCategoryDto>();
        readCategoryDto.Should().BeEquivalentTo(fakePaymentCategory);
    }

    [Fact]
    public async Task Get_EmptyCategoryId_ReturnsBadRequest()
    {
        var response = await client.GetAsync($"{Endpoints.PaymentCategory}/{Guid.Empty}");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_NonExistingCategoryId_ReturnsNotFound()
    {
        var response = await client.GetAsync($"{Endpoints.PaymentCategory}/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    //[Fact]
    //public async Task Get_NoCategories_ReturnsOkWithEmptyCollection()
    //{
    //    dbContext.PaymentCategories.RemoveRange(dbContext.PaymentCategories);
    //    await dbContext.SaveChangesAsync();

    //    var response = await client.GetAsync($"{Endpoints.PaymentCategory}");
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);

    //    var categoryCollection = await response.Content.ReadFromJsonAsync<IEnumerable<ReadPaymentCategoryDto>>();
    //    categoryCollection.Should().HaveCount(0);
    //}

    [Fact]
    public async Task Get_WithAnyCategories_ReturnsOkWithCategoryCollection()
    {
        var fakePaymentCategoryCollection = new PaymentCategoryFaker().Generate(2);
        await dbContext.AddRangeAsync(fakePaymentCategoryCollection);
        await dbContext.SaveChangesAsync();

        var response = await client.GetAsync($"{Endpoints.PaymentCategory}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoryCollection = await response.Content.ReadFromJsonAsync<IEnumerable<ReadPaymentCategoryDto>>();
        categoryCollection.Should().HaveCountGreaterThanOrEqualTo(fakePaymentCategoryCollection.Count);
        foreach (var category in fakePaymentCategoryCollection)
        {
            categoryCollection.Should().ContainEquivalentOf(category);
        }
    }
}