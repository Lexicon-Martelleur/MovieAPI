namespace MovieCardAPI.Model.Exceptions;

public class TokenExpiredException : APIException
{
    public TokenExpiredException() :
        base("Token have expired", "Bad Request")
    { }
}
