using Bll.Interfaces;
using Bll.Models;
using Dal.Interfaces;
using Dal.Models;
using User = Dal.Models.User;

namespace Bll.Services;

public class CourseService(IUserRepository userRepository, ICourseRepository courseRepository) : ICourseService
{
    public async Task<IEnumerable<Course>> GetAllCourses()
    {
        return await courseRepository.GetAllAsync();
    }

    public async Task<Course?> GetCourseById(int id)
    {
        return await courseRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Course>> GetCoursesByStudent(int? studentId)
    {
        if (studentId is null) return Enumerable.Empty<Course>();
        return await courseRepository.GetCoursesByStudentAsync(studentId.Value);
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructor(int? instructorId)
    {
        if (instructorId is null) return Enumerable.Empty<Course>();
        return await courseRepository.GetCoursesByInstructorAsync(instructorId.Value);
    }

    public async Task<Course> CreateCourse(CreateCourseDto courseDto)
    {
        var c = new Course
        {
            Name = courseDto.Name,
            Description = courseDto.Description,
        };
        return await courseRepository.CreateAsync(c);
    }

    public async Task<bool> UpdateCourse(UpdateCourseDto courseDto)
    {
        var c = new Course
        {
            Name = courseDto.Name,
            Description = courseDto.Description,
        };
        return await courseRepository.UpdateAsync(c);
    }

    public async Task<bool> DeleteCourse(int id)
    {
        return await courseRepository.DeleteByIdAsync(id);
    }

    public async Task<bool> EnrollStudentToCourseAsync(int courseId, int studentId)
    {
        (Course? course, User? student) = await GetCourseAndUserWithRoleAsync(courseId, studentId, Role.Student);
        if (course is null || student is null) return false;
        course.Instructor = student;
        return await courseRepository.UpdateAsync(course);
    }

    public async Task<bool> AssignInstructorAsync(int courseId, int instructorId)
    {
        (Course? course, User? instructor) =
            await GetCourseAndUserWithRoleAsync(courseId, instructorId, Role.Instructor);
        if (course is null || instructor is null) return false;
        course.Instructor = instructor;
        return await courseRepository.UpdateAsync(course);
    }

    public async Task<bool> RemoveStudentToCourseAsync(int courseId, int studentId)
    {
        (Course? course, User? student) = await GetCourseAndUserWithRoleAsync(courseId, studentId, Role.Student);
        if (course is null || student is null) return false;
        course.Students.Remove(student);
        return await courseRepository.UpdateAsync(course);
    }

    public async Task<bool> RemoveInstructorAsync(int courseId)
    {
        Course? course = await courseRepository.GetByIdAsync(courseId);
        if (course is null) return false;
        course.Instructor = null;
        return await courseRepository.UpdateAsync(course);
    }

    private async Task<(Course?, User?)> GetCourseAndUserWithRoleAsync(int courseId, int userId, Role role)
    {
        Task<Course?> courseTask = courseRepository.GetByIdAsync(courseId);
        Task<User?> instructorTask = userRepository.GetByIdAsync(userId);
        Course? course = await courseTask;
        User? user = await instructorTask;
        if (user is null || user.Role != role) return (null, null);
        return (course, user);
    }
}