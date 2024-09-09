using Microsoft.EntityFrameworkCore;
using MovieCardAPI.Entities;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class DirectorRepository(MovieContext context) :
    BaseRepository<Director>(context), IDirectorRepository
{
    private MovieContext context = context;

    public async Task<bool> IsExistingDirector(int id)
    {
        return await ThisDbSet.FirstOrDefaultAsync(i => i.Id == id) != null;
    }

    public async Task<ContactInformation?> GetContactInformation(int directorId)
    {
        return await ThisDbSet
            .Where(director => director.Id == directorId)
            .Select(director => director.ContactInformation)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Director>> GetAllDirectors()
    {
        return await FindAll(false).ToListAsync();
    }
}