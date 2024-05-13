using Bll.Models;
using Dal.Models;

namespace Bll.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseListDto>> GetAllCourses();
    Task<Course?> GetCourseById(int id);
    Task<IEnumerable<Course>> GetCoursesByStudent(int? studentId);
    Task<IEnumerable<Course>> GetCoursesByInstructor(int? instructorId);
    Task<Course> CreateCourse(CreateCourseDto courseDto);
    Task<bool> UpdateCourse(UpdateCourseDto courseDto);
    Task<bool> DeleteCourse(int id);
    Task<bool> EnrollStudentToCourseAsync(int courseId, int studentId);
    Task<bool> AssignInstructorAsync(int courseId, int instructorId);
    Task<bool> RemoveStudentToCourseAsync(int courseId, int studentId);
    Task<bool> RemoveInstructorAsync(int courseId);
}