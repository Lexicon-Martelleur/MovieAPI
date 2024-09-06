using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IActorRepository : IBaseRepository<Actor>
{
    Task<bool> IsExistingActors(IEnumerable<int> ids);

    Task<IEnumerable<Actor>> GetActors(int movieId);
}
