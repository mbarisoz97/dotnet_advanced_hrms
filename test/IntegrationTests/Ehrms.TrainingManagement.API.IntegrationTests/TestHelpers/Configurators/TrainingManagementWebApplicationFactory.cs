using MassTransit;
using Testcontainers.MsSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Ehrms.TrainingManagement.API.Database.Context;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ehrms.Shared.TestHepers;
using Docker.DotNet;
using Polly.Retry;
using Polly;
using Microsoft.Data.SqlClient;

namespace Ehrms.TrainingManagement.API.IntegrationTests.TestHelpers.Configurators;

public class TrainingManagementWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private int Port = PortNumberProvider.GetPortNumber();
    private readonly MsSqlContainer _msSqlContainer;

    private readonly AsyncRetryPolicy _retryPolicy = Policy.Handle<DockerApiException>()
        .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
        onRetry: (response, timespan, retryCount, context) =>
        {
            Console.WriteLine($"Retry {retryCount} after {timespan.Seconds}");
        });

    private readonly RetryPolicy _databaseCreationRetryPolicy = Policy.Handle<SqlException>()
    .WaitAndRetry(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
        onRetry: (response, timespan, retryCount, context) =>
        {
            Console.WriteLine($"Retry {retryCount} after {timespan.Seconds}");
        });

    public TrainingManagementWebApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder()
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
            _databaseCreationRetryPolicy.Execute(() =>
            {
                dbContext.Database.EnsureCreated();
            });
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
        await _retryPolicy.ExecuteAsync(async () =>
        {
            await _msSqlContainer.StartAsync();
        });
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