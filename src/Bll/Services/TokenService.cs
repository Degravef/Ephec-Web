using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Bll.Interfaces;
using Dal.Interfaces;
using Dal.Models;
using Microsoft.IdentityModel.Tokens;

namespace Bll.Services;

public class TokenService(IJwtConfig config) : ITokenService
{
    public static class Claims
    {
        public const string Name = "unique_name";
        public const string Role = "Role";
    }

    public string CreateToken(User user)
    {
        DateTime notBefore = DateTime.UtcNow;
        DateTime expires = notBefore.AddMinutes(config.AccessExpirationMinutes);
        SigningCredentials credentials = GetSigningCredentials();
        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsIdentity claims = GetClaimsIdentity(user);
        JwtSecurityToken? token = tokenHandler.CreateJwtSecurityToken(
            issuer: config.Issuer,
            audience: config.Audience,
            subject: claims,
            notBefore: notBefore,
            expires: expires,
            signingCredentials: credentials
        );
        if (token is not null) return tokenHandler.WriteToken(token);
        throw new AuthenticationException("Token could not be created");
    }

    public object? GetClaimValue(string jwtToken, string claimType)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken? token = tokenHandler.ReadJwtToken(jwtToken);
        if (token is null || !token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256)) return null;
        if (token.ValidTo < DateTime.UtcNow) return null;
        return token.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
    }

    private static ClaimsIdentity GetClaimsIdentity(User user)
    {
        List<Claim> claims =
        [
            new Claim(Claims.Name, user.Username)
        ];
        if (user.Role is not null)
        {
            claims.Add(new Claim(Claims.Role, user.Role.ToString()!));
        }

        return new ClaimsIdentity(claims);
    }

    private SigningCredentials GetSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret)),
            SecurityAlgorithms.HmacSha256);
    }
}