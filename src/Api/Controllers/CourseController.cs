using System.Diagnostics.CodeAnalysis;
using Bll.Interfaces;
using Bll.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/")]
public partial class CourseController(
    ICourseService courseService,
    ITokenService tokenService) : ControllerBase
{
    [HttpGet("Courses")]
    public async Task<IActionResult> GetAllCourses()
    {
        return Ok(await courseService.GetAllCourses());
    }

    [HttpGet("UserCourses")]
    public async Task<IActionResult> GetUserCourses()
    {
        Role? role = GetRoleFromBearerToken();
        if (role != Role.Instructor && role != Role.Admin && role != Role.Student) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        IEnumerable<CourseListDto> result = GetRoleFromBearerToken() switch
        {
            Role.Student => await courseService.GetAllCoursesByStudent(GetUserIdFromBearerToken()!.Value),
            Role.Instructor => await courseService.GetAllCoursesByInstructor(GetUserIdFromBearerToken()!.Value),
            _ => await courseService.GetAllCourses()
        };
        Console.WriteLine(result.Count());
        return Ok(result);
    }

    [HttpGet("Course/{id:int}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        return Ok(await courseService.GetCourseById(id));
    }

    [HttpPost("Course")]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto courseDto)
    {
        Role? role = GetRoleFromBearerToken();
        if (role != Role.Instructor && role != Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await courseService.CreateCourse(courseDto));
    }

    [HttpPut("Course")]
    public async Task<IActionResult> UpdateCourse([FromBody] UpdateCourseDto courseDto)
    {
        Role? role = GetRoleFromBearerToken();
        if (role != Role.Instructor && role != Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        bool ok = await courseService.UpdateCourse(courseDto);
        return ok ? Ok() : BadRequest();
    }

    [HttpDelete("Course/{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        Role? role = GetRoleFromBearerToken();
        if (role != Role.Instructor && role != Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        bool ok = await courseService.DeleteCourse(id);
        return ok ? Ok() : BadRequest();
    }

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