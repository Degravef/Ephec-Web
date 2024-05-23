using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class CourseController
{
    [HttpPost("course/assign/{courseId:int}")]
    public async Task<IActionResult> AssignInstructor(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? instructorId = GetUserIdFromBearerToken();
        if (role != Role.Instructor || instructorId is null) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return await courseService.AssignInstructorAsync(courseId, instructorId.Value) ? Ok() : BadRequest();
    }

    [HttpDelete("course/assign/{courseId:int}")]
    public async Task<IActionResult> RemoveInstructor(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? instructorId = GetUserIdFromBearerToken();
        if (role != Role.Instructor || instructorId is null) return Unauthorized();
        if (!ModelState.IsValid) return BadRequest(ModelState);
        // instructors can only remove instructor from their own courses
        if ((await courseService.GetCoursesByInstructor(instructorId.Value)).All(x => x.Id != courseId)) return BadRequest();
        return await courseService.RemoveInstructorAsync(courseId) ? Ok() : BadRequest();
    }
}