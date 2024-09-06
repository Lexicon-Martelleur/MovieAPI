using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IDirectorRepository : IBaseRepository<Director>
{
    Task<bool> IsExistingDirector(int id);

    Task<ContactInformation?> GetContactInformation(int directorId);
}
