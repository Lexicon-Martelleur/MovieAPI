namespace MovieCardAPI.Model.Exeptions;

public class MovieNotFoundException : NotFoundException
{
    private static readonly string _reosurce = "Movie";

    public MovieNotFoundException(
        int id) : 
        base(GetMsg(_reosurce, id)) { }

    public MovieNotFoundException(
        string id) :
        base(GetMsg(_reosurce, id)) { }

    public MovieNotFoundException(
        Guid id) :
        base(GetMsg(_reosurce, id)) { }
}
