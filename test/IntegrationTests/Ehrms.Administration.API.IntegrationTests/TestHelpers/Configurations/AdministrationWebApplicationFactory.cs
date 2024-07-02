using DotNet.Testcontainers.Builders;
using Ehrms.Administration.API.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Configurations;

public class AdministrationWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly int Port = Random.Shared.Next(1000, 60000);
    private readonly MsSqlContainer _msSqlContainer;

    public AdministrationWebApplicationFactory()
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
            services.RemoveAll(typeof(DbContextOptions<AdministrationDbContext>));
            services.AddAdministrationApi();

            services.AddDbContext<AdministrationDbContext>(options =>
            {
                options.UseSqlServer(_msSqlContainer.GetConnectionString(),
                    opt => opt.EnableRetryOnFailure());
            });

            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureCreated();
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
        await _msSqlContainer.StartAsync();
    }

    public async new Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}