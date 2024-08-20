using Ehrms.Authentication.API.Handlers.Auth.Commands;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public sealed class RefreshAuthenticationCommandFaker : Faker<RefreshAuthenticationCommand>
{
    public RefreshAuthenticationCommandFaker()
    {
        RuleFor(x => x.AccessToken, f => f.Random.AlphaNumeric(100));
        RuleFor(x => x.RefreshToken, f => f.Random.AlphaNumeric(20));
    }

    public RefreshAuthenticationCommandFaker WithAccessToken(string accessToken)
    {
        RuleFor(x=>x.AccessToken, accessToken);
        return this;
    }
    
    public RefreshAuthenticationCommandFaker WithRefreshToken(string refreshToken)
    {
        RuleFor(x => x.RefreshToken, refreshToken);
        return this;
    }
}