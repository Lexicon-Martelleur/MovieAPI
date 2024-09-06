using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class UnitOfWork(
    MovieContext context,
    Lazy<IActorRepository> actorRepository,
    Lazy<IContactInformationRepository> contactInformationRepository,
    Lazy<IDirectorRepository> directorRepository,
    Lazy<IGenreRepository> genreRepository,
    Lazy<IMovieRepository> movieRepository,
    Lazy<IMovieGenreRepository> movieGenreRepository,
    Lazy<IMovieRoleRepository> movieRoleRepository) : IUnitOfWork
{
    private readonly MovieContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IActorRepository ActorRepository => actorRepository.Value;

    public IContactInformationRepository ContactInformationRepository => contactInformationRepository.Value;

    public IDirectorRepository DirectorRepository => directorRepository.Value;

    public IGenreRepository GenreRepository => genreRepository.Value;

    public IMovieRepository MovieRepository => movieRepository.Value;

    public IMovieGenreRepository MovieGenreRepository => movieGenreRepository.Value;

    public IMovieRoleRepository MovieRoleRepository => movieRoleRepository.Value;

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}