using Ehrms.Web.Models;

namespace Ehrms.Web.Client;

internal sealed class ProjectServiceClient : IProjectServiceClient
{
	private readonly IEndpointProvider _endpointProvider;
	private readonly IHttpClientFactoryWrapper _httpClientFactoryWrapper;

	public ProjectServiceClient(IHttpClientFactoryWrapper httpClientFactoryWrapper, IEndpointProvider endpointProvider)
	{
		_httpClientFactoryWrapper = httpClientFactoryWrapper;
		_endpointProvider = endpointProvider;
	}


	public async Task<Response<IEnumerable<ProjectModel>>> GetProjectsAsync()
	{
		var client = await _httpClientFactoryWrapper.CreateClient("ApiGateway");
		var response = await client.GetAsync(_endpointProvider.ProjectEndpoint);

		return new Response<IEnumerable<ProjectModel>>()
		{
			StatusCode = response.StatusCode,
			Content = await response.GetContentAs<IEnumerable<ProjectModel>>()
		};
	}
}