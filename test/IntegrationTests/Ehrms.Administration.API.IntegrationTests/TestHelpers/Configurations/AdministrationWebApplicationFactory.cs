using Testcontainers.MsSql;
using Ehrms.Shared.TestHepers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Retry;
using Docker.DotNet;
using Microsoft.Data.SqlClient;
using Ehrms.Administration.API.Database.Context;
using MassTransit;
using System.Data.Common;
using Respawn;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Configurations;

[CollectionDefinition(nameof(AdministrationWebApplicationFactory))]
public class SharedTestCollection : ICollectionFixture<AdministrationWebApplicationFactory>
{
}

public class AdministrationWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    private readonly int Port = PortNumberProvider.GetPortNumber();
    private readonly MsSqlContainer _msSqlContainer;

    private readonly AsyncRetryPolicy _retryPolicy = Policy.Handle<DockerApiException>()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            onRetry: (response, timespan, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} after {timespan.Seconds}");
            });

    public AdministrationWebApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithPortBinding(Port, 1433)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AdministrationDbContext>));
            services.AddDbContext<AdministrationDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString(),
                    opt => opt.EnableRetryOnFailure());
            });

            services.AddMassTransitTestHarness();
        });
    }

    private static AdministrationDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AdministrationDbContext>();

        return dbContext ?? throw new NullReferenceException("DbContext is null");
    }

    private async Task InitializeRespawner()
    {
        _dbConnection = new SqlConnection(_msSqlContainer.GetConnectionString());
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["dbo"]
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            await _msSqlContainer.StartAsync();
        });
        await InitializeRespawner();
    }

    public async new Task DisposeAsync()
    {
        try
        {
            await _msSqlContainer.StopAsync();
        }
        finally
        {
            PortNumberProvider.ReleasePortNumber(Port);
        }
    }
}