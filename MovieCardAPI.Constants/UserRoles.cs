namespace MovieCardAPI.Constants;

public class UserRoles
{
    public const string ADMIN = "Admin";

    public const string USER = "User";

    public static readonly IEnumerable<string> ALL_ROLES = [ADMIN, USER];
}
