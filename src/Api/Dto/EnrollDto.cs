using System.ComponentModel.DataAnnotations;

namespace Api.Dto;

public record EnrollDto
{
    [Required]
    public int CourseId { get; set; }
    [Required]
    public int UserId { get; set; }
}