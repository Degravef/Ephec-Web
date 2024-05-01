namespace Bll.Models;

public record CreateCourseDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}