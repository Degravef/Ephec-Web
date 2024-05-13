using Bll.Interfaces;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class CourseController
{

    [HttpPost("enroll/{courseId:int}")]
    public async Task<IActionResult> EnrollStudentToCourse(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? studentId = GetUserIdFromBearerToken();
        if (role != Role.Student || studentId is null) return this.Unauthorized();
        return await courseService.EnrollStudentToCourseAsync(courseId, studentId.Value) ? this.Ok() : this.BadRequest();
    }

    [HttpDelete("enroll/{courseId:int}")]
    public async Task<IActionResult> RemoveStudentFromCourse(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? studentId = GetUserIdFromBearerToken();
        if (role != Role.Student || studentId is null) return this.Unauthorized();
        return await courseService.RemoveStudentToCourseAsync(courseId, studentId.Value)? this.Ok() : this.BadRequest();
    }
}