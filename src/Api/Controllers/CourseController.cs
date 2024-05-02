using Bll.Interfaces;
using Bll.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class CourseController(ICourseService courseService, ITokenService tokenService)
    : AuthorizationController(tokenService)
{
    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        try
        {
            return UserRole switch
            {
                Role.Student => base.Ok(await courseService.GetCoursesByStudent(UserId)),
                Role.Instructor => base.Ok(await courseService.GetCoursesByInstructor(UserId)),
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
            return Ok(await courseService.GetCourseById(id));
        }
        catch (Exception)
        {
            return Problem();
        }
    }
}