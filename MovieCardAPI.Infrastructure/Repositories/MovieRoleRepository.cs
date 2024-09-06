using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class MovieRoleRepository(MovieContext context) :
    BaseRepository<MovieRole>(context), IMovieRoleRepository
{
    private MovieContext context = context;

    public void CreateMovieRoles(int movieId, IEnumerable<int> actorIds)
    {
        foreach (var id in actorIds)
        {
            ThisDbSet.Add(new MovieRole()
            {
                MovieId = movieId,
                ActorId = id,
            });
        }
    }

    public async Task RemoveMovieRoles(int movieId)
    {
        ThisDbSet.RemoveRange(await ThisDbSet
            .Where(item => item.MovieId == movieId)
            .ToListAsync()
        );
    }

    public void UpdateMovieRoles(
        IEnumerable<int> newActorIds,
        int movieId)
    {
        foreach (var actorId in newActorIds)
        {
            ThisDbSet.Add(new()
            {
                MovieId = movieId,
                ActorId = actorId
            });
        }
    }
}