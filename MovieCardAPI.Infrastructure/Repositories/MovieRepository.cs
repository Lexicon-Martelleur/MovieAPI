using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Entities;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class MovieRepository : BaseRepository<Movie>, IMovieRepository
{
    public MovieRepository(MovieContext context) : base(context) {}

    public async Task<Movie?> GetMovie(int id)
    {
        return await ThisDbSet
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Movie>> GetMovies()
    {
        return await ThisDbSet.ToListAsync();
    }

    public void CreateMovie(
        Movie movie,
        IEnumerable<int> actorIds,
        IEnumerable<int> genreIds)
    {
        ThisDbSet.Add(movie);
    }

    public async Task DeleteMovie(int id)
    {
        var movieEntity = await ThisDbSet
            .Where(item => item.Id == id)
            .FirstOrDefaultAsync();

        if (movieEntity == default)
        {
            return;
        }
        ThisDbSet.Remove(movieEntity);
    }

    public async Task<Director?> GeMovieDirector(int movieId)
    {
        return await ThisDbSet
            .Where(movie => movie.Id == movieId)
            .Select(movie => movie.Director)
            .FirstOrDefaultAsync();
    }
}
