using Bll.Interfaces;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public abstract class ControllerAuth(ITokenService tokenService) : ControllerBase
{
    private string? GetBearerToken()
    {
        try
        {
            string s = HttpContext.Request.Headers.Authorization.ToString();
            if (!s.StartsWith("Bearer ")) return null;
            if (!tokenService.ValidateToken(s[7..])) return null;
            return s[7..];
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected Role? GetRoleFromBearerToken()
    {
        try
        {
            string? token = GetBearerToken();
            if (token is null) return null;
            string? roleString = tokenService.GetClaimValue(token, ITokenService.Claims.Role);
            if (roleString is null) return null;
            return (Role)Enum.Parse(typeof(Role), roleString);
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected int? GetUserIdFromBearerToken()
    {
        try
        {
            string? token = GetBearerToken();
            if (token is null) return null;
            string? idString = tokenService.GetClaimValue(token, ITokenService.Claims.Id);
            if (idString is null) return null;
            if (!int.TryParse(idString, out int id)) return null;
            return id;
        }
        catch (Exception)
        {
            return null;
        }
    }
}