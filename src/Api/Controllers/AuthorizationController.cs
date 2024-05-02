using Bll.Interfaces;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public abstract class AuthorizationController : ControllerBase
{
    private readonly ITokenService _tokenService;
    protected Role? UserRole { get; private set; }
    protected int? UserId { get; private set; }

    protected AuthorizationController(ITokenService tokenService)
    {
        _tokenService = tokenService;
        UserRole = GetRoleFromBearerToken();
        UserId = GetUserIdFromBearerToken();
    }


    private string? GetBearerToken()
    {
        try
        {
            return HttpContext.Request.Headers.Authorization.ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private Role? GetRoleFromBearerToken()
    {
        try
        {
            string? token = GetBearerToken();
            if (token is null) return null;
            string? roleString = _tokenService.GetClaimValue(token, ITokenService.Claims.Role);
            if (roleString is null) return null;
            return (Role)Enum.Parse(typeof(Role), roleString);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private int? GetUserIdFromBearerToken()
    {
        try
        {
            string? token = GetBearerToken();
            if (token is null) return null;
            string? idString = _tokenService.GetClaimValue(token, ITokenService.Claims.Id);
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