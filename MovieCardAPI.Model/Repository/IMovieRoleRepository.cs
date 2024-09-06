namespace MovieCardAPI.Model.Repository;

public interface IMovieRoleRepository : IBaseRepository
{
    void CreateMovieRoles(int movieId, IEnumerable<int> actorIds);

    Task RemoveMovieRoles(int movieId);

    void UpdateMovieRoles(IEnumerable<int> newActorIds, int movieId);
}