using Ehrms.Web.Models;
using Ehrms.Web.Routing;

namespace Ehrms.Web.Client.ProjectApi.ProjectInfo;

internal sealed class ProjectServiceClient : IProjectServiceClient
{
    private readonly IEndpointProvider _endpointProvider;
    private readonly IHttpClientFactoryWrapper _httpClientFactoryWrapper;

    public ProjectServiceClient(IHttpClientFactoryWrapper httpClientFactoryWrapper, IEndpointProvider endpointProvider)
    {
        _httpClientFactoryWrapper = httpClientFactoryWrapper;
        _endpointProvider = endpointProvider;
    }

    public async Task<Response<ProjectModel>> CreateProjectAsync(ProjectModel project)
    {
        var client = await _httpClientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.PutAsJsonAsync(_endpointProvider.ProjectManagementApiEndpoint, project);

        return new Response<ProjectModel>()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<ProjectModel>()
        };
    }

    public async Task<Response<Guid>> DeleteProjectAsync(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.DeleteAsync($"{_endpointProvider.ProjectManagementApiEndpoint}/{id}");

        return new Response<Guid>()
        {
            StatusCode = response.StatusCode,
            Content = id
        };
    }

    public async Task<Response<ProjectModel>> GetProjectAsync(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.GetAsync($"{_endpointProvider.ProjectManagementApiEndpoint}/{id}");

        return new Response<ProjectModel>()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<ProjectModel>()
        };
    }

    public async Task<Response<IEnumerable<ProjectEmploymentModel>>> GetProjectHistoryAsync(Guid id)
    {
        var client = await _httpClientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.GetAsync($"{_endpointProvider.EmploymentApiEndpoint}/{id}");

        return new()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<ProjectEmploymentModel>>()
        };
    }

    public async Task<Response<IEnumerable<ProjectModel>>> GetProjectsAsync()
    {
        var client = await _httpClientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.GetAsync(_endpointProvider.ProjectManagementApiEndpoint);

        return new Response<IEnumerable<ProjectModel>>()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<ProjectModel>>()
        };
    }

    public async Task<Response<ProjectModel>> UpdateProjectAsync(ProjectModel project)
    {
        var client = await _httpClientFactoryWrapper.CreateClient(HttpClients.BackendApiGateway);
        var response = await client.PostAsJsonAsync(_endpointProvider.ProjectManagementApiEndpoint, project);

        return new Response<ProjectModel>()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<ProjectModel>()
        };
    }
}