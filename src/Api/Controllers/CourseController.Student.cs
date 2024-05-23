using Bll.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public partial class CourseController
{
    [HttpGet("EnrolledCourses")]
    public async Task<IActionResult> GetEnrolledCourses()
    {
        Console.WriteLine(GetRoleFromBearerToken());
        Console.WriteLine(GetUserIdFromBearerToken());
        Console.WriteLine(courseService);
        var result = GetRoleFromBearerToken() switch
        {
            Role.Student => await courseService.GetCoursesByStudent(GetUserIdFromBearerToken()!.Value),
            Role.Instructor => await courseService.GetCoursesByInstructor(GetUserIdFromBearerToken()!.Value),
            _ => new List<CourseListDto>(),
        };
        return Ok(result);
    }

    [HttpPost("course/enroll/{courseId:int}")]
    public async Task<IActionResult> EnrollStudentToCourse(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? studentId = GetUserIdFromBearerToken();
        if (role != Role.Student || studentId is null) return Unauthorized();
        return await courseService.EnrollStudentToCourseAsync(courseId, studentId.Value) ? Ok() : BadRequest();
    }

    [HttpDelete("course/enroll/{courseId:int}")]
    public async Task<IActionResult> RemoveStudentFromCourse(int courseId)
    {
        Role? role = GetRoleFromBearerToken();
        int? studentId = GetUserIdFromBearerToken();
        if (role != Role.Student || studentId is null) return Unauthorized();
        return await courseService.RemoveStudentToCourseAsync(courseId, studentId.Value) ? Ok() : BadRequest();
    }
}