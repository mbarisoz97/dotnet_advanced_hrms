using Testcontainers.MsSql;
using Ehrms.Shared.TestHepers;
using Ehrms.Administration.API.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly.Retry;
using Docker.DotNet;
using Microsoft.Data.SqlClient;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Configurations;

public class AdministrationWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
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
            services.AddAdministrationApi();

            services.AddDbContext<AdministrationDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString(),
                    opt => opt.EnableRetryOnFailure());
            });

            var dbContext = CreateDbContext(services);

            _databaseCreationRetryPolicy.Execute(() =>
            {
                dbContext.Database.EnsureCreated();
            });
        });
    }

    private static AdministrationDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AdministrationDbContext>();

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