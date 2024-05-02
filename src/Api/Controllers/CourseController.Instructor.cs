using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class CourseController
{
    [HttpPost("assign/{courseId:int}")]
    public async Task<IActionResult> AssignInstructor(int courseId)
    {
        if (UserRole is not Role.Instructor || UserId is null) return base.Unauthorized();
        return await courseService.AssignInstructorAsync(courseId, UserId.Value) ? base.Ok() : base.BadRequest();
    }

    [HttpDelete("remove/{courseId:int}")]
    public async Task<IActionResult> RemoveInstructor(int courseId)
    {
        if (UserRole is null) return base.Unauthorized();
        if (UserId is null) return base.Unauthorized();
        if (UserRole is not (Role.Instructor or Role.Admin)) return base.Unauthorized();
        if (UserRole is Role.Instructor && !await IsOwnCourse(courseId, UserId.Value))
        {
            return base.Unauthorized();
        }
        return await courseService.RemoveInstructorAsync(courseId) ? base.Ok() : base.BadRequest();
    }

    private async Task<bool> IsOwnCourse(int courseId, int instructorId)
    {
        return (await courseService.GetCoursesByInstructor(instructorId)).All(x => x.Id != courseId);
    }
}