using Dal.Models;

namespace Bll.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
    object? GetClaimValue(string jwtToken, string claimType);
}