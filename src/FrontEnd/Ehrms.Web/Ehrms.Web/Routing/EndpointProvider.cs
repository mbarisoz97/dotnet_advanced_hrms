namespace Ehrms.Web;

internal sealed class EndpointProvider : IEndpointProvider
{
	public string AutheticationService => "/api/Account/Login";
}
