using DotNet.Testcontainers.Builders;
using Ehrms.ProjectManagement.API.Context;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace Ehrms.ProjectManagement.API.IntegrationTests.TestHelpers.Configurations;

public class ProjectManagementWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private int Port => Random.Shared.Next(1024, 49151);
    private readonly MsSqlContainer _msSqlContainer;

    public ProjectManagementWebApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server")
            .WithEnvironment("MSSQL_SA_PASSWORD", "yourStrong(!)Password")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithPortBinding(Port, 1433)

            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ProjectDbContext>));
            services.AddProjectManagementApi();

            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString(),
                    opt => opt.EnableRetryOnFailure());
            });

            services.AddMassTransitTestHarness();

            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureCreated();
        });
    }

    private ProjectDbContext CreateDbContext(IServiceCollection services)
    {
        var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ProjectDbContext>();

        return dbContext;
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    public async new Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}
