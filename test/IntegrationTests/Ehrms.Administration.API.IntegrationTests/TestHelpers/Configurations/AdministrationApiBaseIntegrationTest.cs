using Ehrms.Administration.API.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Ehrms.Administration.API.IntegrationTests.TestHelpers.Configurations;

public abstract class AdministrationApiBaseIntegrationTest : IClassFixture<AdministrationWebApplicationFactory>
{
    protected readonly HttpClient client;
    protected readonly AdministrationDbContext dbContext;

    protected AdministrationApiBaseIntegrationTest(AdministrationWebApplicationFactory factory)
    {
        client = factory.CreateClient();
        var scope = factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<AdministrationDbContext>();    
    }
}