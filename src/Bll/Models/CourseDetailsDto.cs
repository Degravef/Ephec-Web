namespace Bll.Models;

public record CourseDetailsDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Instructor { get; set; }
}