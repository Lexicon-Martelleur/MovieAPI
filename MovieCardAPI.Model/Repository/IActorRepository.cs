using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IActorRepository : IBaseRepository
{
    Task<IEnumerable<Actor>> GetMovieRoles(int movieId);

}
