namespace MovieCardAPI.Model.Exceptions;

public class DomainUnableOperationException : APIException
{
    public DomainUnableOperationException(string message) :
        base(message, "Bad Request") { }
}
