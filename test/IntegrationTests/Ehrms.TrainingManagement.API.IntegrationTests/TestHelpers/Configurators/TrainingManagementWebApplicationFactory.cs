using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Ehrms.TrainingManagement.API.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ehrms.TrainingManagement.API.IntegrationTests.TestHelpers.Configurators;

internal static class CustomDbContextFactory
{
    internal static TrainingDbContext Create(string databaseName)
    {
        TrainingDbContext projectDbContext = new(new DbContextOptionsBuilder<TrainingDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options);

        return projectDbContext;
    }
}

internal class TrainingManagementWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string DatabaseName = "TrainingManamagementDb";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<TrainingDbContext>));
            services.AddTrainingManagementApi();

            services.AddDbContext<TrainingDbContext>(options =>
            {
                options.UseInMemoryDatabase(DatabaseName);
            });

            var dbContext = CreateDbContext(services);
        });
    }
    
    private TrainingDbContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<TrainingDbContext>();

        return dbContext ?? throw new NullReferenceException("DbContext is null");
    }
}