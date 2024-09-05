using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MovieContext _context;
    
    private readonly Lazy<IMovieRepository> _movieRepository;

    public IMovieRepository MovieRepository => _movieRepository.Value;

    public UnitOfWork(MovieContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _movieRepository = new Lazy<IMovieRepository>(() => new MovieRepository(context));
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}