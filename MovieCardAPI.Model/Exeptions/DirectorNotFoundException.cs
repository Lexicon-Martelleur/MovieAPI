namespace MovieCardAPI.Model.Exeptions;

public class DirectorNotFoundException : NotFoundException
{
    private static readonly string _reosurce = "Director";

    public DirectorNotFoundException(
        int id) : 
        base(GetMsg(_reosurce, id)) { }

    public DirectorNotFoundException(
        string id) :
        base(GetMsg(_reosurce, id)) { }


    public DirectorNotFoundException(
        Guid id) :
        base(GetMsg(_reosurce, id)) { }
}
