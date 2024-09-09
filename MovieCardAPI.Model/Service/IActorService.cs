using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Service;

public interface IActorService
{
    Task<IEnumerable<ActorDTO>> GetActors();
}
