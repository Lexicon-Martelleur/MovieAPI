namespace MovieCardAPI.Model.Repository;

public interface IUnitOfWork
{
    IMovieRepository MovieRepository { get; }

    IActorRepository ActorRepository { get; }

    Task<bool> SaveChangesAsync();
}
