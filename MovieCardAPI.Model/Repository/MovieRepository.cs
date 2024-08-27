using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;
using MovieCardAPI.DB.Contexts;

namespace MovieCardAPI.Model.Repository;

public class MovieRepository : IMovieRepository
{

    private readonly MovieContext _context;
    public MovieRepository(MovieContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Movie?> GetMovie(int id)
    {
        return await _context.Movies
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
        return await _context.Movies.ToListAsync();
    }
}
