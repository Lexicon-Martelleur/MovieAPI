namespace MovieCardAPI.Model.Repository;

public interface IUnitOfWork
{
    IMovieRepository MovieRepository { get; }

    Task<bool> SaveChangesAsync();

}
