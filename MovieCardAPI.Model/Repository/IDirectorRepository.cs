using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IDirectorRepository : IBaseRepository
{
    Task<bool> IsExistingDirector(int id);

    Task<ContactInformation?> GetContactInformation(int directorId);
}
