namespace MovieCardAPI.Model.Exceptions;

public abstract class APIException(
    string message,
    string title) : Exception(message)
{
    public string Title { get; } = title;
}
