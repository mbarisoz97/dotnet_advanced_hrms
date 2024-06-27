using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Ehrms.TrainingManagement.API.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;
using DotNet.Testcontainers.Builders;
using MassTransit;

namespace Ehrms.TrainingManagement.API.IntegrationTests.TestHelpers.Configurators;

internal class TrainingManagementWebApplicationFactory : WebApplicationFactory<Program>
{
    private int Port => Random.Shared.Next(1000, 60000);
    private readonly MsSqlContainer _msSqlContainer;

    public TrainingManagementWebApplicationFactory()
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
            services.RemoveAll(typeof(DbContextOptions<TrainingDbContext>));
            services.AddTrainingManagementApi();

            services.AddDbContext<TrainingDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString(),
                    opt => opt.EnableRetryOnFailure());
            });

            services.AddMassTransitTestHarness();
        });
    }

    public TrainingDbContext CreateDbContext()
    {
        var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<TrainingDbContext>();

        return dbContext ?? throw new NullReferenceException("DbContext is null");
    }

    public async Task Start()
    {
        await _msSqlContainer.StartAsync();
        try
        {
            var context = CreateDbContext();
            await context.Database.EnsureCreatedAsync();
        }
        catch
        {
            await Stop();
        }
    }

    public async Task Stop()
    {
        await _msSqlContainer.StopAsync();
    }
}