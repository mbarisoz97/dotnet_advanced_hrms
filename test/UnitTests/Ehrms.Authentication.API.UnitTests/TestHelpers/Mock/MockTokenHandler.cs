using Ehrms.Shared;
using System.Security.Claims;

namespace Ehrms.Authentication.API.UnitTests.TestHelpers.Mock;

public sealed class MockTokenHandler : Mock<ITokenHandler>
{
    public void SetupGetPrincipalFromExpiredToken(User user)
    {
        if (user == null)
        {
            throw new Exception("Null user is not allowed");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
        };

        claims.AddRange(user.UserRoles.Select(x => new Claim(ClaimTypes.Role, x.Role!.Name)));

        var claimsIdentity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        SetupGetPrincipalFromExpiredToken(claimsPrincipal);
    }

    private void SetupGetPrincipalFromExpiredToken(ClaimsPrincipal claimsPrincipal)
    {
        Setup(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>()))
            .Returns(claimsPrincipal);
    }

    public void SetupGenerate(GenerateTokenResponse? response = null)
    {
        Setup(x => x.Generate(It.IsAny<GenerateJwtRequest>()))
            .Returns(response);
    }
}