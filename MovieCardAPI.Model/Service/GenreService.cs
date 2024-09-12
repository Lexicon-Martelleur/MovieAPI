using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Repository;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI.Model.Service;

public class GenreService : IGenreService
{
    private readonly IUnitOfWork _uow;

    private readonly ICustomMapper _mapper;

    public GenreService(IUnitOfWork uow, ICustomMapper mapper)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<GenreDTO>> GetGenres()
    {
        var genreEntities = await _uow.GenreRepository.GetAllGenres();
        return _mapper.MapGenreEntitiesToGenreDTOs(genreEntities);
    }
}
