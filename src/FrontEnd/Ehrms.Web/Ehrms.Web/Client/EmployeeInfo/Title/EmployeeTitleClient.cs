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

    public async Task<Response<EmployeeTitleModel>> CreateTitle(EmployeeTitleModel employeeTitle)
    {
        var client = await _httpClientFactory.CreateClient("ApiGateway");
        var response = await client.PutAsJsonAsync(BaseEndpoint, employeeTitle);

        return new Response<EmployeeTitleModel>
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<EmployeeTitleModel>()
        };
    }

    public async Task<Response<IEnumerable<EmployeeTitleModel>>> GetAllTitles()
    {
        var client = await _httpClientFactory.CreateClient("ApiGateway");
        var response = await client.GetAsync(BaseEndpoint);

        return new Response<IEnumerable<EmployeeTitleModel>>
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<EmployeeTitleModel>>()
        };
    }

    public async Task<Response<EmployeeTitleModel>> GetTitleById(Guid Id)
    {
        var client = await _httpClientFactory.CreateClient("ApiGateway");
        var response = await client.GetAsync($"{BaseEndpoint}/{Id}");

        return new Response<EmployeeTitleModel>
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<EmployeeTitleModel>()
        };
    }
}
