using Dal.Models;

namespace Bll.Models;

public record UserListDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public Role Role { get; set; }
}