using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

internal class GenreRepository(MovieContext context) :
    BaseRepository<Genre>(context), IGenreRepository
{
    private MovieContext context = context;

    public async Task<bool> IsExistingGenres(IEnumerable<int> ids)
    {
        var matchingIds = await ThisDbSet
            .Where(genre => ids.Contains(genre.Id))
            .Select(genre => genre.Id)
            .ToListAsync();

        return matchingIds.Count == ids.Distinct().ToList().Count;
    }

    public async Task<IEnumerable<Genre>> GetGenres(int movieId)
    {
        return await Context.MovieGenres
            .Where(moviGenre => moviGenre.MovieId == movieId)
            .Join(
                ThisDbSet,
                moviGenre => moviGenre.GenreId,
                genre => genre.Id,
                (moviGenre, genre) => genre
            )
            .ToListAsync();
    }
}