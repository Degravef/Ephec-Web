using Bll.Interfaces;
using Bll.Models;
using Dal.Interfaces;
using Dal.Models;
using User = Dal.Models.User;

namespace Bll.Services;

public class CourseService(IUserRepository userRepository, ICourseRepository courseRepository) : ICourseService
{
    public async Task<IEnumerable<CourseListDto>> GetAllCourses()
    {
        return (await courseRepository.GetAllAsync())
            .Select(c => new CourseListDto
            {
                Id = c.Id,
                Name = c.Name,
                IsEnrolled = false
            });
    }

    public async Task<IEnumerable<CourseListDto>> GetAllCoursesByStudent(int userId)
    {
        var allCoursesTask = courseRepository.GetAllAsync();
        var enrolledCoursesTask = courseRepository.GetCoursesByStudentAsync(userId);
        (IEnumerable<Course> allCourses, IEnumerable<Course> enrolledCourses) =
            (await allCoursesTask, await enrolledCoursesTask);
        var enrolledCourseIds = new HashSet<int>(enrolledCourses.Select(c => c.Id));
        return allCourses.Select(c => new CourseListDto
        {
            Id = c.Id,
            Name = c.Name,
            IsEnrolled = enrolledCourseIds.Contains(c.Id)
        });
    }

    public async Task<CourseDetailsDto?> GetCourseById(int id)
    {
        Course? course = await courseRepository.GetByIdAsync(id);
        if (course is null) return null;
        return new CourseDetailsDto
        {
            Id = course.Id,
            Name = course.Name,
            Description = course.Description ?? string.Empty,
            Instructor = course.Instructor?.Username ?? string.Empty,
        };
    }

    public async Task<IEnumerable<CourseListDto>> GetCoursesByStudent(int studentId)
    {
        return (await courseRepository.GetCoursesByStudentAsync(studentId))
            .Select(c => new CourseListDto
            {
                Id = c.Id,
                Name = c.Name,
                IsEnrolled = true
            });
    }

    public async Task<IEnumerable<CourseListDto>> GetCoursesByInstructor(int instructorId)
    {
        return (await courseRepository.GetCoursesByInstructorAsync(instructorId))
            .Select(c => new CourseListDto
            {
                Id = c.Id,
                Name = c.Name,
                IsEnrolled = true
            });
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
            Id = courseDto.Id,
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
        course.Students.Add(student);
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
        IEnumerable<Course> courses = await courseRepository.GetCoursesByStudentAsync(studentId);
        Course? course = courses.FirstOrDefault(c => c.Id == courseId);
        if (course is null) return false;
        User? student = course.Students.FirstOrDefault(s => s.Id == studentId);
        if (student is null) return false;
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