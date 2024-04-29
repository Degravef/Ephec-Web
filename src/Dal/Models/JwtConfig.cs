using Dal.Interfaces;

namespace Dal.Models;

public class JwtConfig : IJwtConfig
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int AccessExpirationMinutes { get; set; }
}
