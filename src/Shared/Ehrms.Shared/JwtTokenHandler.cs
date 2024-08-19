using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Ehrms.Shared;

public interface ITokenHandler
{
    GenerateTokenResponse? Generate(GenerateJwtRequest request);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}

public class JwtTokenHandler : ITokenHandler
{
    internal const string JWT_SECURITY_KEY = "d4iNJCVb1o14QaHlHKP5Isw/2VFa5OT3mLQeSb0WHLM=";
    internal const uint JWT_TOKEN_VALIDITY_MINS = 20;

    public GenerateTokenResponse? Generate(GenerateJwtRequest request)
    {
        var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, request.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var role in request.Roles)
        {
            if (role != null)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }
        }

        var claimsIdentity = new ClaimsIdentity(claims);

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(tokenKey);
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
        SecurityTokenDescriptor securityTokenDescriptor = new()
        {
            Issuer = "https://localhost:7203",
            Audience = "https://localhost:7203",
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = signingCredentials
        };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return new GenerateTokenResponse()
        {
            AccessToken = token,
            ExpiresIn = tokenExpiryTimeStamp,
        };
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var validation = new TokenValidationParameters
        {
            ValidIssuer = "https://localhost:7203",
            ValidAudience = "https://localhost:7203",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_SECURITY_KEY)),
            ValidateLifetime = false
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }
}