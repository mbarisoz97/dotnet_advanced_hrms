﻿using Docker.DotNet;
using DotNet.Testcontainers.Builders;
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

namespace Ehrms.ProjectManagement.API.IntegrationTests.TestHelpers.Configurations;

public class ProjectManagementWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
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
