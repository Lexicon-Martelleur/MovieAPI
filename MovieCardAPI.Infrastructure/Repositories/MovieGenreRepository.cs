using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class MovieGenreRepository(MovieContext context) :
    BaseRepository<MovieGenre>(context), IMovieGenreRepository
{
    private MovieContext context = context;

    public void CreateMovieGenres(int movieId, IEnumerable<int> genreIds)
    {
        foreach (var id in genreIds)
        {
            ThisDbSet.Add(new MovieGenre()
            {
                MovieId = movieId,
                GenreId = id
            });
        }
    }

    public async Task RemoveMovieGenres(int movieId)
    {
        ThisDbSet.RemoveRange(await ThisDbSet
            .Where(item => item.MovieId == movieId)
            .ToListAsync()
        );
    }

    public void UpdateMovieGenres(
        IEnumerable<int> newGenreIds,
        int movieId)
    {
        foreach (var genreId in newGenreIds)
        {
            ThisDbSet.Add(new()
            {
                MovieId = movieId,
                GenreId = genreId
            });
        }
    }
}
