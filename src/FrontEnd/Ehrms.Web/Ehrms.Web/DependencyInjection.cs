using Ehrms.Shared;
using Ehrms.Web.Client;

namespace Ehrms.Web;

internal static class DependencyInjection
{
	internal static IServiceCollection AddWebUi(this IServiceCollection services)
	{
		services.AddSingleton<IEndpointProvider, EndpointProvider>();

		services.AddTransient<IEmployeeServiceClient, EmployeeInfoServiceClient>();
		services.AddScoped<ITokenHandler, JwtTokenHandler>();

		return services;
	}
}