using Testcontainers.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Ehrms.Shared.TestHepers;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Configurations;

public class AuthenticationWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly int Port = PortNumberProvider.GetPortNumber();
    private readonly MsSqlContainer _msSqlContainer;

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

            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureCreated();
        });
    }

    private static ApplicationUserDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationUserDbContext>();

        return dbContext ?? throw new NullReferenceException("DbContext is null");
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    public async new Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
        PortNumberProvider.ReleasePortNumber(Port);
    }
}