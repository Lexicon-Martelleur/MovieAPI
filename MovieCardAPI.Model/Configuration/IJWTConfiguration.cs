namespace MovieCardAPI.Model.Configuration;

public interface IJWTConfiguration
{
    string Audience { get; set; }
    int Expires { get; set; }
    string Issuer { get; set; }
}