using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class UnitOfWork(MovieContext context) : IUnitOfWork
{
    private readonly MovieContext _context = context ?? throw new ArgumentNullException(nameof(context));

    private readonly Lazy<IActorRepository> _actorRepository = new(() =>
    new ActorRepository(context));

    private readonly Lazy<IContactInformationRepository> _contactInformationRepository = new(() =>
        new ContactInformationRepository(context));

    private readonly Lazy<IDirectorRepository> _directorRepository = new(() =>
        new DirectorRepository(context));

    private readonly Lazy<IGenreRepository> _genreRepository = new(() =>
        new GenreRepository(context));

    private readonly Lazy<IMovieRepository> _movieRepository = new(() =>
    new MovieRepository(context));

    private readonly Lazy<IMovieGenreRepository> _movieGenreRepository = new(() =>
        new MovieGenreRepository(context));

    private readonly Lazy<IMovieRoleRepository> _movieRoleRepository = new(() =>
        new MovieRoleRepository(context));

    public IActorRepository ActorRepository => _actorRepository.Value;

    public IContactInformationRepository ContactInformationRepository => _contactInformationRepository.Value;

    public IDirectorRepository DirectorRepository => _directorRepository.Value;

    public IGenreRepository GenreRepository => _genreRepository.Value;

    public IMovieRepository MovieRepository => _movieRepository.Value;

    public IMovieGenreRepository MovieGenreRepository => _movieGenreRepository.Value;

    public IMovieRoleRepository MovieRoleRepository => _movieRoleRepository.Value;

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}