using Ehrms.Web.Models;
using Ehrms.Web.Routing;

namespace Ehrms.Web.Client.EmployeeApi.Skill;

internal sealed class SkillServiceClient : ISkillServiceClient
{
    private readonly IEndpointProvider _endpointProvider;
    private readonly IHttpClientFactoryWrapper _clientFactoryWrapper;

    public SkillServiceClient(IHttpClientFactoryWrapper clientFactoryWrapper, IEndpointProvider endpointProvider)
    {
        _clientFactoryWrapper = clientFactoryWrapper;
        _endpointProvider = endpointProvider;
    }

    public async Task<Response<IEnumerable<SkillModel>>> GetSkillsAsync()
    {
        var client = await _clientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync(_endpointProvider.EmployeeSkillServiceEndpoint);

        return new Response<IEnumerable<SkillModel>>()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<IEnumerable<SkillModel>>()
        };
    }

    public async Task<Response<SkillModel>> CreateSkillAsync(SkillModel skill)
    {
        var client = await _clientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.PutAsJsonAsync(_endpointProvider.EmployeeSkillServiceEndpoint, skill);

        return new Response<SkillModel>()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<SkillModel>()
        };
    }

    public async Task<Response<SkillModel>> GetSkillAsync(Guid id)
    {
        var client = await _clientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.GetAsync($"{_endpointProvider.EmployeeSkillServiceEndpoint}/{id}");

        return new Response<SkillModel>()
        {
            StatusCode = response.StatusCode,
            Content = await response.GetContentAs<SkillModel>()
        };
    }

    public async Task<Response<Guid>> DeleteEmployeeAsync(Guid id)
    {
        var client = await _clientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.DeleteAsync($"{_endpointProvider.EmployeeSkillServiceEndpoint}/{id}");

        return new Response<Guid>()
        {
            StatusCode = response.StatusCode,
            Content = id
        };
    }

    public async Task<Response<SkillModel>> UpdateSkillAsync(SkillModel skill)
    {
        var client = await _clientFactoryWrapper.CreateClient("ApiGateway");
        var response = await client.PostAsJsonAsync(_endpointProvider.EmployeeSkillServiceEndpoint, skill);

        return await response.AsCustomResponse<SkillModel>();
    }
}
