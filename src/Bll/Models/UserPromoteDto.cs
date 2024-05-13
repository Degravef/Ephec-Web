using System.ComponentModel.DataAnnotations;
using Dal.Models;

namespace Bll.Models;

public record UserPromoteDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [EnumDataType(typeof(Role))]
    public Role Role { get; set; }
}