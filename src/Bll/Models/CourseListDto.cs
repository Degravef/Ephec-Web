namespace Bll.Models;

public record CourseListDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
}