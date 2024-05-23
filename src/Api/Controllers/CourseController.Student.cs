using Bll.Interfaces;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class CourseController
{
    [HttpGet("UserCourses")]
    public async Task<IActionResult> GetUserCourses()
    {
        try
        {
            return GetRoleFromBearerToken() switch
            {
                Role.Student => base.Ok(await courseService.GetCoursesByStudent(GetUserIdFromBearerToken())),
                Role.Instructor => base.Ok(await courseService.GetCoursesByInstructor(GetUserIdFromBearerToken())),
                _ => base.Ok(await courseService.GetCoursesByStudent(GetUserIdFromBearerToken()))
            };
        }
        catch (Exception)
        {
            return base.Problem();
        }
    }

    [HttpPost("course/enroll/{courseId:int}")]
    public async Task<IActionResult> EnrollStudentToCourse(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? studentId = GetUserIdFromBearerToken();
        if (role != Role.Student || studentId is null) return this.Unauthorized();
        return await courseService.EnrollStudentToCourseAsync(courseId, studentId.Value) ? this.Ok() : this.BadRequest();
    }

    [HttpDelete("course/enroll/{courseId:int}")]
    public async Task<IActionResult> RemoveStudentFromCourse(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? studentId = GetUserIdFromBearerToken();
        if (role != Role.Student || studentId is null) return this.Unauthorized();
        return await courseService.RemoveStudentToCourseAsync(courseId, studentId.Value)? this.Ok() : this.BadRequest();
    }
}