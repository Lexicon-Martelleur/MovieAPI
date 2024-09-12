using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Repository;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI.Model.Service;

public class ActorService : IActorService
{
    private readonly IUnitOfWork _uow;

    private readonly ICustomMapper _mapper;

    public ActorService(IUnitOfWork uow, ICustomMapper mapper)
    {
        _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ActorDTO>> GetActors()
    {
        var actorEntities = await _uow.ActorRepository.GetAllActors();
        return _mapper.MapActorEntitiesToActorDTOs(actorEntities);
    }
}
