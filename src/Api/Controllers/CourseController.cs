using Bll.Interfaces;
using Bll.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class CourseController(ICourseService courseService,
    ITokenService tokenService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        try
        {
            return GetRoleFromBearerToken() switch
            {
                Role.Student => this.Ok(await courseService.GetCoursesByStudent(GetUserIdFromBearerToken())),
                Role.Instructor => this.Ok(await courseService.GetCoursesByInstructor(GetUserIdFromBearerToken())),
                _ => this.Ok(await courseService.GetAllCourses())
            };
        }
        catch (Exception)
        {
            return this.Problem();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        try
        {
            return this.Ok(await courseService.GetCourseById(id));
        }
        catch (Exception)
        {
            return this.Problem();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto courseDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest(this.ModelState);
        try
        {
            return this.Ok(await courseService.CreateCourse(courseDto));
        }
        catch (Exception)
        {
            return this.Problem();
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseDto courseDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest(this.ModelState);
        try
        {
            return this.Ok(await courseService.UpdateCourse(courseDto));
        }
        catch (Exception)
        {
            return this.Problem();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        try
        {
            return this.Ok(await courseService.DeleteCourse(id));
        }
        catch (Exception)
        {
            return this.Problem();
        }
    }

    private string? GetBearerToken()
    {
        try
        {
            return this.HttpContext.Request.Headers.Authorization.ToString();
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