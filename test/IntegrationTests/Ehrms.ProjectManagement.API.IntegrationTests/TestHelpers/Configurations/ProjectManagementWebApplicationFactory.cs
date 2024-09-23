using Docker.DotNet;
using Ehrms.ProjectManagement.API.Database.Context;
using Ehrms.Shared.TestHepers;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Retry;
using Polly;
using Testcontainers.MsSql;
using Respawn;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace Ehrms.ProjectManagement.API.IntegrationTests.TestHelpers.Configurations;

[CollectionDefinition(nameof(ProjectManagementWebApplicationFactory))]
public class SharedTestCollection : ICollectionFixture<ProjectManagementWebApplicationFactory>
{
}

public class ProjectManagementWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
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

    public ProjectManagementWebApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder()
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
        });
    }

    private ProjectDbContext CreateDbContext(IServiceCollection services)
    {
        var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetService<ProjectDbContext>();

        if (dbContext == null)
        {
            throw new ArgumentException("Could not resolve dbcontext");
        }

        return dbContext;
    }

    public async Task InitializeAsync()
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            await _msSqlContainer.StartAsync();
        });
        await InitializeRespawner();
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
