using Ehrms.EmployeeInfo.API.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ehrms.EmployeeInfo.API.IntegrationTests;

internal class EmployeeInfoWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<EmployeeInfoDbContext>));
            services.AddEmployeeInfoApi();

            services.AddDbContext<EmployeeInfoDbContext>(options =>
            {
                options.UseInMemoryDatabase("EmployeeInfoDb");
            });

            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });

            var dbContext = CreateDbContext(services);
            dbContext.Database.EnsureDeleted();

            Seed.InitializeTestDb(dbContext);
        });
    }

    private EmployeeInfoDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<EmployeeInfoDbContext>();

        return dbContext ?? throw new NullReferenceException("DbContext is null");
    }
}