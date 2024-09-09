using MovieCardAPI.Model.DTO;

namespace MovieCardAPI.Model.Service;

public interface IDirectorService
{
    Task<IEnumerable<DirectorDTO>> GetDirectors();
}
