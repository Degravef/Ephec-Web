using Bll.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class CourseController
{

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto courseDto)
    {
        if (UserRole is not Role.Admin) return base.Unauthorized();
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
        if (UserRole is not Role.Admin) return base.Unauthorized();
        if (!ModelState.IsValid) return base.BadRequest(ModelState);
        try
        {
            return base.Ok(await courseService.UpdateCourse(courseDto));
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        if (UserRole is not Role.Admin) return base.Unauthorized();
        try
        {
            return base.Ok(await courseService.DeleteCourse(id));
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }
    [HttpPost("assign/{courseId:int}/{instructorId:int}")]
    public async Task<IActionResult> AssignInstructor(int courseId, int instructorId)
    {
        if (UserRole is not Role.Admin) return base.Unauthorized();
        return await courseService.AssignInstructorAsync(courseId, instructorId) ? base.Ok() : base.BadRequest();
    }
    
    [HttpPost("enroll/{courseId:int}/{studentId:int}")]
    public async Task<IActionResult> EnrollStudentToCourse(int courseId, int studentId)
    {
        if (UserRole is not Role.Admin) return base.Unauthorized();
        return await courseService.EnrollStudentToCourseAsync(courseId, studentId) ? base.Ok() : base.BadRequest();
    }

    [HttpPost("remove/{courseId:int}/{studentId:int}")]
    public async Task<IActionResult> RemoveStudentFromCourse(int courseId, int studentId)
    {
        if (UserRole is not Role.Admin) return base.Unauthorized();
        return await courseService.RemoveStudentToCourseAsync(courseId, studentId)? base.Ok() : base.BadRequest();
    }
}