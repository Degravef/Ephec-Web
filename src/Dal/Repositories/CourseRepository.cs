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

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
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
        context.Courses.Update(course);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Course course)
    {
        User? existingUser = await context.Users.FindAsync(course.Id);
        if (existingUser is null) return false;
        context.Entry(existingUser).CurrentValues.SetValues(course);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        Course? entity = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (entity is not null) return await DeleteAsync(entity);
        return false;
    }

    public async Task<IEnumerable<Course>> GetCoursesByStudentAsync(int userId)
    {
        User? currentUser = await context.Users
                                         .Include(u => u.CoursesAsStudent)
                                         .FirstOrDefaultAsync(x => x.Id == userId);
        if (currentUser is null) return new List<Course>();
        return currentUser.CoursesAsStudent;
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(int userId)
    {
        User? currentUser = await context.Users
                                         .Include(u => u.CoursesAsInstructor)
                                         .FirstOrDefaultAsync(x => x.Id == userId);
        if (currentUser is null) return new List<Course>();
        return currentUser.CoursesAsInstructor;
    }
}