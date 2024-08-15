using Ehrms.Shared;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Middleware;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.API.PipelineBehaviors;
using Microsoft.AspNetCore.Identity;

namespace Ehrms.Authentication.API;

internal static class DependencyInjection
{
    internal static IServiceCollection AddAuthenticationApi(this IServiceCollection services)
    {
        services.AddScoped<MigrationManager>();
        services.AddScoped<ApplicationUserDbSeed>();
        services.AddScoped<DbContext, ApplicationUserDbContext>();
        
        services.AddTransient<ITokenHandler, JwtTokenHandler>();
        services.AddScoped<IUserManagerAdapter, UserManagerAdapter>();

        services.AddScoped<GlobalExceptionHandlingMiddleware>();

        var assembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly)
             .AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        return services;
    }

    internal static async Task CheckDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;
        var migrationManager = services.GetRequiredService<MigrationManager>();

        await migrationManager.Init();
    }

    internal static async Task SeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;
        var dbInitializer = services.GetRequiredService<ApplicationUserDbSeed>();

        await dbInitializer.SeedAsync();
    }

    internal static async Task AddUserRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();

        var roleNames = Enum.GetValues(typeof(UserRoles))
            .Cast<UserRoles>()
            .Select(x=>x.ToString());

        foreach (string? roleName in roleNames)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                logger.LogInformation("Skipped null or empty role name");
                continue;
            }

            if (await roleManager.RoleExistsAsync(roleName))
            {
                logger.LogInformation("Skipped existing identity role : <{roleName}>", roleName);
                continue;
            }

            var identityResult = await roleManager.CreateAsync(new Role(roleName));
            if (!identityResult.Succeeded)
            {
                logger.LogError("Could not add user role : <{roleName}>", roleName);
                foreach (var item in identityResult.Errors)
                {
                    logger.LogError(item.Description);
                }
            }
            else
            {
                logger.LogInformation("Added user identity role : <{roleName}>", roleName);
            }
        }
    }
}