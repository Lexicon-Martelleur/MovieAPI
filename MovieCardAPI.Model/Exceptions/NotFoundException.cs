namespace MovieCardAPI.Model.Exceptions;

public abstract class NotFoundException(
    string message, 
    string title = "Not Found") : APIException(message, title)
{
    protected static string GetMsg<IdType>(
        string resource,
        IdType id) =>
        $"{resource} with id '{id}' was not found";

    protected static string GetMsg(
        string resource) =>
        $"{resource} was not found";
}
