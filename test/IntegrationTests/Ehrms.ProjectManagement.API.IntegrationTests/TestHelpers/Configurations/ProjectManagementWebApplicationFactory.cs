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

internal class ProjectManagementWebApplicationFactory : WebApplicationFactory<Program>
{
    private int Port => Random.Shared.Next(1000, 60000);
    private readonly MsSqlContainer _msSqlContainer;

    public ProjectManagementWebApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server")
            .WithEnvironment("MSSQL_SA_PASSWORD", "yourStrong(!)Password")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithPortBinding(Port, 1433)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(1433))
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
        });
    }

    private void EnsureDatabaseCreated()
    {
        var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ProjectDbContext>();
        dbContext!.Database.EnsureCreated();
    }

    public async Task Start()
    {
        await _msSqlContainer.StartAsync();
        
        try
        {
            EnsureDatabaseCreated();
        }
        catch
        {
            await Stop();
        }
    }

    public async Task Stop()
    {
        await _msSqlContainer.StopAsync();
        await _msSqlContainer.DisposeAsync().AsTask();
    }
}