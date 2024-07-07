using Ehrms.Web.Client;

namespace Ehrms.Web;

internal static class DependencyInjection
{
	internal static IServiceCollection AddWebUi(this IServiceCollection services)
	{
		services.AddTransient<BearerTokenHandler>();
		services.AddSingleton<ITokenProvider, TokenProvider>();
		services.AddSingleton<IEndpointProvider, EndpointProvider>();

		services.AddScoped<IEmployeeServiceClient, EmployeeInfoServiceClient>();
		return services;	
	}
}