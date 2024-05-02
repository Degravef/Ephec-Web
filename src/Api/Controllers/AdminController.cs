using Bll.Interfaces;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IUserService userService, ITokenService tokenService)
    : AuthorizationController(tokenService)
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        if (UserRole is not Role.Admin) return base.Unauthorized();
        return base.Ok(await userService.GetAllAsync());
    }

    //promote student to teacher
    [HttpPatch("promote")]
    public async Task<IActionResult> PromoteUser([FromBody] int userId, Role newRole)
    {
        if (UserRole is not Role.Admin) return base.Unauthorized();
        try
        {
            return base.Ok(await userService.PromoteUser(userId, newRole));
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }
}