namespace Dal.Models;

public class User
{
    public int Id { get; init; }
    public required string Username { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public Role? Role { get; set; }
}