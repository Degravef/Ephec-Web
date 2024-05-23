using Bll.Interfaces;
using Bll.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/")]
public partial class CourseController(ICourseService courseService,
    IUserService userService,
    ITokenService tokenService) : ControllerBase
{
    [HttpGet("Courses")]
    public async Task<IActionResult> GetAllCourses()
    {
         return base.Ok(await courseService.GetAllCourses());
    }

    [HttpGet("Course/{id:int}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        return base.Ok(await courseService.GetCourseById(id));
    }

    [HttpPost("Course")]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto courseDto)
    {
        if (!ModelState.IsValid) return base.BadRequest(ModelState);
        return base.Ok(await courseService.CreateCourse(courseDto));
    }

    [HttpPut("Course")]
    public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseDto courseDto)
    {
        if (!ModelState.IsValid) return base.BadRequest(ModelState);
        bool ok = await courseService.UpdateCourse(courseDto);
        return ok ? base.Ok() : base.BadRequest();
    }

    [HttpDelete("Course/{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        bool ok = await courseService.DeleteCourse(id);
        return ok ? base.Ok() : base.BadRequest();
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
            string? roleString = tokenService.GetClaimValue(token, ITokenService.Claims.Role);
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