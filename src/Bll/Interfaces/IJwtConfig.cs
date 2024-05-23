namespace Bll.Interfaces;

public interface IJwtConfig
{
    string? Secret { get; set; }
    string? Issuer { get; set; }
    string? Audience { get; set; }
    int AccessExpirationMinutes { get; set; }
}