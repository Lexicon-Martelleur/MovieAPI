using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IMovieRoleRepository : IBaseRepository<MovieRole>
{
    void CreateMovieRoles(int movieId, IEnumerable<int> actorIds);

    Task RemoveMovieRoles(int movieId);

    void UpdateMovieRoles(IEnumerable<int> newActorIds, int movieId);
}