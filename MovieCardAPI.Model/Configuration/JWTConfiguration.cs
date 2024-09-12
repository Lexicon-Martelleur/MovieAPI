namespace MovieCardAPI.Model.Configuration;

public class JWTConfiguration : IJWTConfiguration
{
    public const string Section = "JwtSettings";
    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
    public int Expires { get; set; }
}
