using Microsoft.Extensions.Configuration;

namespace MovieCardAPI.Constants;

public class AppConfig
{
    public const int RefreshTokenExpireTime = 2;

    public static string GetPassword(
        IConfiguration configuration
    ) => configuration["password"] ??  throw new Exception("password not exist in config");

    public static string GetSecretKey(
        IConfiguration configuration
    ) => configuration["secret_key"] ?? throw new Exception("password not exist in config");

    public static void Validate(IConfiguration configuration)
    {
        GetPassword(configuration);
    }

    public static readonly (string Dev, string Prod) CorsPolicies = (
        Dev: "DevCorsPolicy",
        Prod: "ProdCorsPolicy"
    );
}
