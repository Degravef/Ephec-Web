using Dal.Interfaces;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories;

public class CourseRepository(DataContext context) : ICourseRepository
{
    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await context.Courses.ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int courseId)
    {
        return await context.Courses
                            .Include(course => course.Instructor)
                            .FirstOrDefaultAsync(course => course.Id == courseId);
    }

    public async Task<Course> CreateAsync(Course course)
    {
        await context.Courses.AddAsync(course);
        await context.SaveChangesAsync();
        await context.Entry(course).ReloadAsync();
        return course;
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        Course? existingCourse = await context.Courses.FindAsync(course.Id);
        if (existingCourse is null) return false;
        existingCourse.Name = course.Name;
        existingCourse.Description = course.Description;
        context.Courses.Update(existingCourse);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteByIdAsync(int courseId)
    {
        Course? entity = await context.Courses.FirstOrDefaultAsync(course => course.Id == courseId);
        if (entity is null) return false;
        context.Courses.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Course>> GetCoursesByStudentAsync(int userId)
    {
        User? currentUser = await context.Users
                                         .Include(user => user.CoursesAsStudent)
                                         .FirstOrDefaultAsync(user => user.Id == userId);
        if (currentUser is null) return new List<Course>();
        return currentUser.CoursesAsStudent;
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int userId)
    {
        User? currentUser = await context.Users
                                         .Include(user => user.CoursesAsInstructor)
                                         .FirstOrDefaultAsync(user => user.Id == userId);
        if (currentUser is null) return new List<Course>();
        return currentUser.CoursesAsInstructor;
    }
}