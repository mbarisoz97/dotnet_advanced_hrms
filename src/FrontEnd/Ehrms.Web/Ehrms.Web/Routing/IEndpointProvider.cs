namespace Ehrms.Web;

internal interface IEndpointProvider
{
	string AutheticationEndpoint { get; }
	string RefreshEndpoint { get; }
}
