using Ehrms.Web.Client;

namespace Ehrms.Web;

internal static class DependencyInjection
{
	internal static IServiceCollection AddWebUi(this IServiceCollection services)
	{
		services.AddScoped<IEmployeeInfoServiceClient, EmployeeInfoServiceClient>();
		return services;	
	}
}