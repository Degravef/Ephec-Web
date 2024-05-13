using Bll.Interfaces;
using Bll.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class CourseController(ICourseService courseService,
    IUserService userService,
    ITokenService tokenService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        try
        {
            return GetRoleFromBearerToken() switch
            {
                Role.Student => base.Ok(await courseService.GetCoursesByStudent(GetUserIdFromBearerToken())),
                Role.Instructor => base.Ok(await courseService.GetCoursesByInstructor(GetUserIdFromBearerToken())),
                _ => base.Ok(await courseService.GetAllCourses())
            };
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        try
        {
            return base.Ok(await courseService.GetCourseById(id));
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto courseDto)
    {
        if (!ModelState.IsValid) return base.BadRequest(ModelState);
        try
        {
            return base.Ok(await courseService.CreateCourse(courseDto));
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseDto courseDto)
    {
        if (!ModelState.IsValid) return base.BadRequest(ModelState);
        bool ok = await courseService.UpdateCourse(courseDto);
        return ok ? base.Ok() : base.BadRequest();
    }

    [HttpDelete("{id:int}")]
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