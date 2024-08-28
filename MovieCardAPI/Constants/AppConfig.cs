namespace MovieCardAPI.Constants;

internal class AppConfig
{
    internal static readonly (string Dev, string Prod) CorsPolicies = (
        Dev: "DevCorsPolicy",
        Prod: "ProdCorsPolicy"
    );
}
