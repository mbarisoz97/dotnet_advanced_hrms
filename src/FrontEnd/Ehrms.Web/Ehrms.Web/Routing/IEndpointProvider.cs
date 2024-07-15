namespace Ehrms.Web;

internal interface IEndpointProvider
{
	string AutheticationEndpoint { get; }
	string RefreshEndpoint { get; }
	string EmployeeInfoServiceEndpoint { get; }
	string? ProjectEndpoint { get; }
	string? EmploymentApiEndpoint { get; }
}
