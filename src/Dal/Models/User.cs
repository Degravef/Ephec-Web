namespace Dal.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public Role? Role { get; set; }
    public ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();
    public ICollection<Course> CoursesAsStudent { get; set; } = new List<Course>();
}