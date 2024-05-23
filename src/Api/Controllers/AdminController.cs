using Api.Dto;
using Bll.Interfaces;
using Bll.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AdminController(
    ICourseService courseService,
    IUserService userService,
    ITokenService tokenService) : ControllerAuth(tokenService)
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        if (GetRoleFromBearerToken()!= Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return base.Ok(await userService.GetAllAsync());
    }

    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (GetRoleFromBearerToken()!= Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        bool b = await userService.DeleteByIdAsync(id);
        return b? base.Ok() : base.BadRequest();
    }

    [HttpPut("promote")]
    public async Task<IActionResult> Promote([FromBody] UserPromoteDto userPromote)
    {
        if (GetRoleFromBearerToken()!= Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        bool b = await userService.PromoteAsync(userPromote);
        return b ? base.Ok() : base.BadRequest();
    }

    [HttpPost("enroll")]
    public async Task<IActionResult> EnrollStudentToCourse([FromBody] EnrollDto dto)
    {
        if (GetRoleFromBearerToken()!= Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Role? role = await userService.GetRoleAsync(dto.UserId);
        if (role != Role.Student) return BadRequest();
        return await courseService.EnrollStudentToCourseAsync(dto.CourseId, dto.UserId) ? Ok() : BadRequest();
    }

    [HttpDelete("enroll")]
    public async Task<IActionResult> RemoveStudentFromCourse([FromBody] EnrollDto dto)
    {
        if (GetRoleFromBearerToken()!= Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Role? role = await userService.GetRoleAsync(dto.UserId);
        if (role != Role.Student) return BadRequest();
        return await courseService.RemoveStudentToCourseAsync(dto.CourseId, dto.UserId)? Ok() : BadRequest();
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignInstructor([FromBody] EnrollDto dto)
    {
        if (GetRoleFromBearerToken()!= Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Role? role = await userService.GetRoleAsync(dto.UserId);
        if (role != Role.Instructor) return BadRequest();
        return await courseService.AssignInstructorAsync(dto.CourseId, dto.UserId) ? Ok() : BadRequest();
    }

    [HttpDelete("assign")]
    public async Task<IActionResult> RemoveInstructor([FromBody] EnrollDto dto)
    {
        if (GetRoleFromBearerToken()!= Role.Admin) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        Role? role = await userService.GetRoleAsync(dto.UserId);
        if (role != Role.Instructor) return BadRequest();
        // instructors can only remove instructor from their own courses
        return await courseService.RemoveInstructorAsync(dto.CourseId) ? Ok() : BadRequest();
    }
}