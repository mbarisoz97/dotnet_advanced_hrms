using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Ehrms.Shared;

public interface ITokenHandler
{
    GenerateTokenResponse? Generate(AuthenticationRequest request);
}

public class JwtTokenHandler : ITokenHandler
{
    internal const string JWT_SECURITY_KEY = "d4iNJCVb1o14QaHlHKP5Isw/2VFa5OT3mLQeSb0WHLM=";
    internal const uint JWT_TOKEN_VALIDITY_MINS = 20;

    public GenerateTokenResponse? Generate(AuthenticationRequest request)
    {

        var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
        var claimsIdentity = new ClaimsIdentity(new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Name, request.Username),

        });

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(tokenKey);
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
        SecurityTokenDescriptor securityTokenDescriptor = new()
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = signingCredentials
        };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);

        return new GenerateTokenResponse()
        {
            Token = token,
            Username = request.Username,
            ExpiresIn = tokenExpiryTimeStamp,
        };
    }
}

public static class JwtAuthenticationExtenions
{
    public static void AddCustomJwtAuthentication(this IServiceCollection service)
    {
        service.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = true;
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtTokenHandler.JWT_SECURITY_KEY))
            };
        });
    }
}