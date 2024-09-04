using Ehrms.Web.Models.EmployeeInfo;

namespace Ehrms.Web.Client.EmployeeInfo.Title;

internal class EmployeeTitleClient : IEmployeeTitleClient
{
    private const string BaseEndpoint = "/api/Title";

    private readonly IEndpointProvider _endpointProvider;
    private readonly IHttpClientFactoryWrapper _httpClientFactory;

    public EmployeeTitleClient(IEndpointProvider endpointProvider, IHttpClientFactoryWrapper httpClientFactory)
    {
        _endpointProvider = endpointProvider;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Response<IEnumerable<ReadEmployeeTitleModel>>> GetAllTitles()
    {
        var client = await _httpClientFactory.CreateClient("ApiGateway");
        var response = await client.GetAsync(BaseEndpoint);

        return new Response<IEnumerable<ReadEmployeeTitleModel>>
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<ReadEmployeeTitleModel>>()
        };
    }
}
