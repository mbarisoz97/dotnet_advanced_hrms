namespace Ehrms.Web;

internal sealed class EndpointProvider : IEndpointProvider
{
	public string RefreshEndpoint => "/api/Account/Refresh";
	public string AutheticationEndpoint => "/api/Account/Login";
}
