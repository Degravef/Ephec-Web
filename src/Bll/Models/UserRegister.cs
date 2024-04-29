namespace Bll.Models;

public record UserRegister
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}