namespace MovieCardAPI.Model.Exeptions;

public abstract class NotFoundException(
    string message, 
    string title = "Not Found") : Exception(message)
{
    public string Title { get; } = title;

    protected static string GetMsg<IdType>(
        string resource,
        IdType id) =>
        $"{resource} with id '{id}' was not found";

    protected static string GetMsg(
        string resource) =>
        $"{resource} was not found";
}
