using Ehrms.Shared;

namespace Ehrms.Authentication.TestHelpers.Faker.Models;

public class GenerateTokenResponseFaker : Faker<GenerateTokenResponse>
{
    public GenerateTokenResponseFaker()
    {
        RuleFor(x => x.AccessToken, f => f.Random.Chars(count: 100).ToString());
        RuleFor(x => x.ExpiresIn, f => f.Date.Future());
    }
}