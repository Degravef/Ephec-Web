using Bll.Interfaces;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class CourseController
{
    [HttpPost("assign/{courseId:int}")]
    public async Task<IActionResult> AssignInstructor(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? instructorId = GetUserIdFromBearerToken();
        if (role != Role.Instructor || instructorId is null) return this.Unauthorized();
        return await courseService.AssignInstructorAsync(courseId, instructorId.Value) ? this.Ok() : this.BadRequest();
    }

    [HttpDelete("remove/{courseId:int}")]
    public async Task<IActionResult> RemoveInstructor(string token, int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? instructorId = GetUserIdFromBearerToken();
        if (role != Role.Instructor || instructorId is null) return this.Unauthorized();
        // instructors can only remove instructor from their own courses
        if ((await courseService.GetCoursesByInstructor(instructorId)).All(x => x.Id != courseId)) return this.BadRequest();
        return await courseService.RemoveInstructorAsync(courseId) ? this.Ok() : this.BadRequest();
    }
}