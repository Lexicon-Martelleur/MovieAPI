using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;


namespace MovieCardAPI.Infrastructure.Repositories;

public class ActorRepository(MovieContext context) : 
    BaseRepository<Actor>(context), IActorRepository
{
    public async Task<IEnumerable<Actor>> GetMovieRoles(int movieId)
    {
        return await Context.MovieRoles
            .Where(movieRole => movieRole.MovieId == movieId)
            .Join(
                Context.Actors,
                movieRole => movieRole.ActorId,
                actor => actor.Id,
                (moviRole, actor) => actor
            )
            .ToListAsync();
    }
}
