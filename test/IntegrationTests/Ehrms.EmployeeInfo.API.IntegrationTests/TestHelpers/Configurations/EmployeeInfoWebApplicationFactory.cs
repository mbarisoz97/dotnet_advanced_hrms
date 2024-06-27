using DotNet.Testcontainers.Builders;
using Ehrms.EmployeeInfo.API.Context;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace Ehrms.EmployeeInfo.API.IntegrationTests;

public class EmployeeInfoWebApplicationFactory : WebApplicationFactory<Program>
{
    private int Port = Random.Shared.Next(1000, 60000);

    private readonly MsSqlContainer _msSqlContainer;

    public EmployeeInfoWebApplicationFactory()
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
            services.RemoveAll(typeof(DbContextOptions<EmployeeInfoDbContext>));
            services.AddEmployeeInfoApi();

            services.AddDbContext<EmployeeInfoDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString(),
                    opt => opt.EnableRetryOnFailure());
            });

            services.AddMassTransitTestHarness();
        });
    }

    public async Task Start()
    {
        await _msSqlContainer.StartAsync();

        try
        {
            var scope = this.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<DbContext>();
            await dbContext!.Database.EnsureCreatedAsync();
        }
        catch
        {
            await Console.Out.WriteLineAsync("Test");
        }
    }

    public async Task Stop()
    {
        await _msSqlContainer.StopAsync();
        await _msSqlContainer.DisposeAsync().AsTask();
    }
}