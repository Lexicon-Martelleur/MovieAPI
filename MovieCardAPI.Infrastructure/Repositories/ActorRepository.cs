using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;


namespace MovieCardAPI.Infrastructure.Repositories;

public class ActorRepository(MovieContext context) : 
    BaseRepository<Actor>(context), IActorRepository
{
    public async Task<bool> IsExistingActors(IEnumerable<int> ids)
    {
        var matchingIds = await ThisDbSet
            .Where(actor => ids.Contains(actor.Id))
            .Select(actor => actor.Id)
            .ToListAsync();

        return matchingIds.Count == ids.Distinct().ToList().Count;
    }

    public async Task<IEnumerable<Actor>> GetActors(int movieId)
    {
        return await Context.MovieRoles
            .Where(movieRole => movieRole.MovieId == movieId)
            .Join(
                ThisDbSet,
                movieRole => movieRole.ActorId,
                actor => actor.Id,
                (moviRole, actor) => actor
            )
            .ToListAsync();
    }
}
