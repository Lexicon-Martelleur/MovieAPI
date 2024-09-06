using MovieCardAPI.Model.Repository;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI.Model.Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IMovieService> _movieService;
    
    public IMovieService MovieService => _movieService.Value;

    public ServiceManager(IUnitOfWork uow, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(uow);
        _movieService = new Lazy<IMovieService>(() => new MovieService(uow, mapper));
    }
}
