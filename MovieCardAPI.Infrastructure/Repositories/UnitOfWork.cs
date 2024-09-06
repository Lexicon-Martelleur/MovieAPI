using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class UnitOfWork(MovieContext context) : IUnitOfWork
{
    private readonly MovieContext _context = context ?? throw new ArgumentNullException(nameof(context));
    
    private readonly Lazy<IMovieRepository> _movieRepository = new(() =>
        new MovieRepository(context));

    private readonly Lazy<IActorRepository> _actorRepository = new (() => 
        new ActorRepository(context));

    public IMovieRepository MovieRepository => _movieRepository.Value;

    public IActorRepository ActorRepository => _actorRepository.Value;

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}