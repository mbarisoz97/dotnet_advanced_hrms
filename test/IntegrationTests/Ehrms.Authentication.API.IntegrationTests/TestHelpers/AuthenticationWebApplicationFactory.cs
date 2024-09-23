using Testcontainers.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Ehrms.Shared.TestHepers;
using Docker.DotNet;
using Polly.Retry;
using Polly;
using Respawn;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Configurations;

[CollectionDefinition(nameof(AuthenticationWebApplicationFactory))]
public class SharedTestCollection : ICollectionFixture<AuthenticationWebApplicationFactory>
{
}

public class AuthenticationWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly int Port = PortNumberProvider.GetPortNumber();
    private readonly MsSqlContainer _msSqlContainer;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    private readonly AsyncRetryPolicy _retryPolicy = Policy.Handle<DockerApiException>()
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
        onRetry: (response, timespan, retryCount, context) =>
        {
            Console.WriteLine($"Retry {retryCount} after {timespan.Seconds}");
        });

    public AuthenticationWebApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithPortBinding(Port, 1433)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTest");
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationUserDbContext>));
            services.AddDbContext<ApplicationUserDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString(), options => options.EnableRetryOnFailure());
            });
        });
    }

    private static ApplicationUserDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationUserDbContext>();

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