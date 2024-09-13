namespace MovieCardAPI.Model.Exceptions;

public class ActorNotFoundException : NotFoundException
{
    private static readonly string _reosurce = "Actor";

    public ActorNotFoundException(
        int id) :
       base(GetMsg(_reosurce, id)) { }

    public ActorNotFoundException(
        string id) :
       base(GetMsg(_reosurce, id)) { }

    public ActorNotFoundException(
        Guid id) :
        base(GetMsg(_reosurce, id)) { }
}
