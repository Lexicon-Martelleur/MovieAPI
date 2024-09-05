namespace MovieCardAPI.Model.Exeptions;

public class DomainUnableOperationException : Exception
{
    public DomainUnableOperationException(string message) :
        base(message) { }

    public string Title { get; } = "Bad Request";
}
