﻿using Ehrms.Shared;
using Ehrms.Web.Client;

namespace Ehrms.Web;

internal static class DependencyInjection
{
	internal static IServiceCollection AddWebUi(this IServiceCollection services)
	{
		services.AddSingleton<IEndpointProvider, EndpointProvider>();

		services.AddScoped<IProjectServiceClient, ProjectServiceClient>();
		services.AddScoped<IEmployeeServiceClient, EmployeeInfoServiceClient>();
		services.AddScoped<ISkillServiceClient, SkillServiceClient>();
		services.AddScoped<ITokenHandler, JwtTokenHandler>();
		services.AddScoped<IHttpClientFactoryWrapper, HttpClientFactoryWrapper>();

		return services;
	}
}