using MovieCardAPI.Entities;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Repository;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI.Model.Service;

public class DirectorService : IDirectorService
{
    private readonly IUnitOfWork _uow;

    private readonly IMapper _mapper;

    public DirectorService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<DirectorDTO>> GetDirectors()
    {
        var directorEntities = await _uow.DirectorRepository.GetAllDirectors();
        return _mapper.MapDirectorEntitiesToDirectorDTOs(directorEntities);
    }
}
