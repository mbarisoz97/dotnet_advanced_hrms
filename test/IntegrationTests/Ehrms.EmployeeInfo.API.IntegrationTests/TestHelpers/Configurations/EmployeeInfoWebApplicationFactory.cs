using Docker.DotNet;
using DotNet.Testcontainers.Builders;
using Ehrms.EmployeeInfo.API.Database.Context;
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
using Ehrms.Shared.TestHepers;
using Microsoft.Data.SqlClient;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.TestHelpers.Configurations;

public class EmployeeInfoWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
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

    private readonly RetryPolicy _databaseCreationRetryPolicy = Policy.Handle<SqlException>()
    .WaitAndRetry(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
        onRetry: (response, timespan, retryCount, context) =>
        {
            Console.WriteLine($"Retry {retryCount} after {timespan.Seconds}");
        });

    public EmployeeInfoWebApplicationFactory()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithPortBinding(Port, 1433)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<EmployeeInfoDbContext>));
            services.AddEmployeeInfoApi();

            services.AddDbContext<EmployeeInfoDbContext>(options =>
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

    private static EmployeeInfoDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeInfoDbContext>();

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