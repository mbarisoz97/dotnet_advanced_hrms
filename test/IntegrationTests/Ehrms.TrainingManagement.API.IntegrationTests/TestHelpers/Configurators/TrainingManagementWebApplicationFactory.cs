using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;
using DotNet.Testcontainers.Builders;
using MassTransit;
using Ehrms.TrainingManagement.API.Database.Context;

namespace Ehrms.TrainingManagement.API.IntegrationTests.TestHelpers.Configurators;

public class TrainingManagementWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private int Port => Random.Shared.Next(1024 , 49151);
    private readonly MsSqlContainer _msSqlContainer;

    public TrainingManagementWebApplicationFactory()
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
            services.RemoveAll(typeof(DbContextOptions<TrainingDbContext>));
            services.AddTrainingManagementApi();

            services.AddDbContext<TrainingDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString(),
                    opt => opt.EnableRetryOnFailure());
            });
            services.AddMassTransitTestHarness();

            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureCreated();
        });
    }

    private static TrainingDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrainingDbContext>();

        return dbContext ?? throw new NullReferenceException("DbContext is null");
    }

    public TrainingDbContext CreateDbContext()
    {
        var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TrainingDbContext>();

        return dbContext ?? throw new NullReferenceException("DbContext is null");
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