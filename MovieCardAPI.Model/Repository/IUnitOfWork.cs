namespace MovieCardAPI.Model.Repository;

public interface IUnitOfWork
{
    IActorRepository ActorRepository { get; }

    IContactInformationRepository ContactInformationRepository { get; }

    IDirectorRepository DirectorRepository { get; }

    IGenreRepository GenreRepository { get; }

    IMovieRepository MovieRepository { get; }

    IMovieGenreRepository MovieGenreRepository { get; }

    IMovieRoleRepository MovieRoleRepository { get; }

    Task<bool> SaveChangesAsync();
}
