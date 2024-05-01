namespace Bll.Models;

public record UpdateCourseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}