using Dal.Models;

namespace Dal.Interfaces;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int courseId);
    Task<Course> CreateAsync(Course course);
    Task<bool> UpdateAsync(Course course);
    Task<bool> DeleteAsync(Course course);
    Task<bool> DeleteByIdAsync(int courseId);
    Task<IEnumerable<Course>> GetCoursesByStudentAsync(int userId);
    Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int userId);
}