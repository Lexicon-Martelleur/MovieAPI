using MovieCardAPI.Entities;
using MovieCardAPI.Infrastructure.Contexts;
using MovieCardAPI.Model.Repository;

namespace MovieCardAPI.Infrastructure.Repositories;

public class ContactInformationRepository(MovieContext context) :
    BaseRepository<ContactInformation>(context), IContactInformationRepository
{
    private MovieContext context = context;
}