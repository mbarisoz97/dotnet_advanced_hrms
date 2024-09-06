using Ehrms.EmployeeInfo.API.Database.Context;
using Ehrms.Shared;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Ehrms.EmployeeInfo.API.IntegrationTests.Controllers;

public abstract class BaseEmployeeInfoIntegrationTest : IClassFixture<EmployeeInfoWebApplicationFactory>
{
    protected readonly EmployeeInfoDbContext dbContext;
    protected readonly EmployeeInfoWebApplicationFactory factory;
    protected readonly HttpClient client;

    protected BaseEmployeeInfoIntegrationTest(EmployeeInfoWebApplicationFactory factory)
    {
        this.factory = factory;
        client = this.factory.CreateClient();

        var scope = factory.Services.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<EmployeeInfoDbContext>();

        var request = new GenerateJwtRequest
        {
            Username = "TestUser"
        };
        var jwt = new JwtTokenHandler().Generate(request);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt!.AccessToken);
        this.factory = factory;
    }
}