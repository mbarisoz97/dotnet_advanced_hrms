namespace Ehrms.Web.Client;

public interface IHttpClientFactoryWrapper
{
    Task<HttpClient> CreateClient(string name);
}
