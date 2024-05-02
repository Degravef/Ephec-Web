using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class CourseController
{

    [HttpPost("enroll/{courseId:int}")]
    public async Task<IActionResult> EnrollStudentToCourse(int courseId)
    {
        if (UserRole is not Role.Student || UserId is null) return base.Unauthorized();
        return await courseService.EnrollStudentToCourseAsync(courseId, UserId.Value) ? base.Ok() : base.BadRequest();
    }

    [HttpPost("remove/{courseId:int}")]
    public async Task<IActionResult> RemoveStudentFromCourse(int courseId)
    {
        if (UserRole is not Role.Student || UserId is null) return base.Unauthorized();
        return await courseService.RemoveStudentToCourseAsync(courseId, UserId.Value)? base.Ok() : base.BadRequest();
    }
}