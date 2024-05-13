using Bll.Interfaces;
using Bll.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/")]
public class AccountController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        try
        {
            return base.Ok(await userService.RegisterAsync(userRegister));
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
    {
        try
        {
            return base.Ok(await userService.LoginAsync(userLogin));
        }
        catch (UnauthorizedAccessException)
        {
            return base.Unauthorized();
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }
}