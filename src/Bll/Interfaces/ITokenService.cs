using Dal.Models;

namespace Bll.Interfaces;

public interface ITokenService
{

    public static class Claims
    {
        public const string Id = "id";
        public const string Name = "name";
        public const string Role = "role";
    }

    string CreateToken(User user);
    string? GetClaimValue(string jwtToken, string claimType);
    bool ValidateToken(string s);
}