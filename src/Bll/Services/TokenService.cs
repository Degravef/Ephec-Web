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
    private const string Algorithm = SecurityAlgorithms.HmacSha256;

    private readonly SigningCredentials _credentials = new(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret)), Algorithm);

    public string CreateToken(User user)
    {
        DateTime notBefore = DateTime.UtcNow;
        DateTime expires = notBefore.AddMinutes(config.AccessExpirationMinutes);
        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsIdentity claims = GetClaimsIdentity(user);
        JwtSecurityToken? token = tokenHandler.CreateJwtSecurityToken(
            issuer: config.Issuer,
            audience: config.Audience,
            subject: claims,
            notBefore: notBefore,
            expires: expires,
            signingCredentials: _credentials
        );
        if (token is not null) return tokenHandler.WriteToken(token);
        throw new AuthenticationException("Token could not be created");
    }

    public bool ValidateToken(string jwtToken)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken? token = tokenHandler.ReadJwtToken(jwtToken);
        if (token is null ||
            !token.Header.Alg.Equals(Algorithm) ||
            !token.SigningCredentials.Key.Equals(Encoding.UTF8.GetBytes(config.Secret))) return false;
        if (token.ValidTo < DateTime.UtcNow) return false;
        return true;
    }

    public string? GetClaimValue(string jwtToken, string claimType)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        JwtSecurityToken? token = tokenHandler.ReadJwtToken(jwtToken);
        if (token is null || !token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256)) return null;
        if (token.ValidTo < DateTime.UtcNow) return null;
        return token.Claims.FirstOrDefault(claim => claim.Type == claimType)?.Value;
    }

    private static ClaimsIdentity GetClaimsIdentity(User user)
    {
        return new ClaimsIdentity([
            new Claim(ITokenService.Claims.Id, user.Id.ToString()),
            new Claim(ITokenService.Claims.Name, user.Username),
            new Claim(ITokenService.Claims.Role, user.Role.ToString()!)
        ]);
    }
}