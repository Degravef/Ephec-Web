namespace Dal.Models;

public class Course
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public User? Instructor { get; set; }
    public int? InstructorId { get; set; }
    public ICollection<User> Students { get; set; } = new List<User>();
}