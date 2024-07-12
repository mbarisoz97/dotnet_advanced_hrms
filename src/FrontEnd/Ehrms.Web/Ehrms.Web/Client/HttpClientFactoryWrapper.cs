using BitzArt.Blazor.Cookies;
using Ehrms.Web.Routing;
using System.Net.Http.Headers;

namespace Ehrms.Web.Client;

internal sealed class HttpClientFactoryWrapper : IHttpClientFactoryWrapper
{
    private readonly ICookieService _cookieService;
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpClientFactoryWrapper(IHttpClientFactory httpClientFactory, ICookieService cookieService)
    {
        _cookieService = cookieService;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<HttpClient> CreateClient(string name)
    {
        var client = _httpClientFactory.CreateClient(name);
        var accessToken = await _cookieService.GetAsync(CookieKeys.AccessToken);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken?.Value);

        return client;
    }
}
