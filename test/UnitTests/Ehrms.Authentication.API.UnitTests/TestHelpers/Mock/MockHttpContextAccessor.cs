using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Ehrms.Authentication.API.UnitTests.TestHelpers.Mock;

public sealed class MockHttpContextAccessor : Mock<IHttpContextAccessor>
{
    public MockHttpContextAccessor()
    {
        SetupDefaultHttpContext();
    }

    public void SetupDefaultHttpContext(string username = "MyTestUser")
    {
        var httpContext = GetDefaultHttpContext(username);
        Setup(x => x.HttpContext).Returns(httpContext);
    }

    private static HttpContext GetDefaultHttpContext(string username)
    {
        List<Claim> defaultClaims = [new Claim(ClaimTypes.Name, username)];
        ClaimsIdentity defaultClaimsIdentity = new(defaultClaims);
        ClaimsPrincipal defaultClaimsPrincipal = new(defaultClaimsIdentity);
        HttpContext defaultHttpContext = new DefaultHttpContext();
        defaultHttpContext.User = defaultClaimsPrincipal;

        return defaultHttpContext;
    }

    public void SetupHttpContextForUser(string username)
    {
        SetupDefaultHttpContext(username);
    }
}