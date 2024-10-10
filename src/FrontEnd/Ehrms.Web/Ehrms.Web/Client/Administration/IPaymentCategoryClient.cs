﻿using Ehrms.Web.Models.Administration;

namespace Ehrms.Web.Client.Administration;

public interface IPaymentCategoryClient
{
    Task<Response<IEnumerable<PaymentCategoryModel>>> GetPaymentCategories();
    Task<Response<PaymentCategoryModel>> GetPaymentCategoryById(Guid id);
    Task<Response<PaymentCategoryModel>> CreatePaymentCategory(PaymentCategoryModel paymentCategory);
}

public class PaymentCategoryClient : IPaymentCategoryClient
{
    private const string PaymentCategoryEndpoint = "/api/PaymentCategory";

    private readonly IHttpClientFactoryWrapper _httpClientFactoryWrapper;

    public PaymentCategoryClient(IHttpClientFactoryWrapper httpClientFactoryWrapper)
    {
        _httpClientFactoryWrapper = httpClientFactoryWrapper;
    }

    public async Task<Response<PaymentCategoryModel>> CreatePaymentCategory(PaymentCategoryModel paymentCategory)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.PutAsJsonAsync(PaymentCategoryEndpoint, paymentCategory);

        return new Response<PaymentCategoryModel>
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<PaymentCategoryModel>()
        };
    }

    public async Task<Response<IEnumerable<PaymentCategoryModel>>> GetPaymentCategories()
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync(PaymentCategoryEndpoint);

        return new Response<IEnumerable<PaymentCategoryModel>>
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<PaymentCategoryModel>>()
        };
    }

    public async Task<Response<PaymentCategoryModel>> GetPaymentCategoryById(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync($"{PaymentCategoryEndpoint}/{id}");

        return new Response<PaymentCategoryModel>
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<PaymentCategoryModel>()
        };
    }
}