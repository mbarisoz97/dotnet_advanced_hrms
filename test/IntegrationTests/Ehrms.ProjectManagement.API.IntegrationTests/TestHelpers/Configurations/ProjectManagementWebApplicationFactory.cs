using Ehrms.ProjectManagement.API.Context;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ehrms.ProjectManagement.API.IntegrationTests.TestHelpers.Configurations;

internal class ProjectManagementWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryTestHarness harness = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ProjectDbContext>));
            services.AddProjectManagementApi();

            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseInMemoryDatabase("ProjectManamagementDb");
            });

            var dbContext = CreateDbContext(services);
        });
    }

    private ProjectDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ProjectDbContext>();

        return dbContext ?? throw new NullReferenceException("DbContext is null");
    }
}