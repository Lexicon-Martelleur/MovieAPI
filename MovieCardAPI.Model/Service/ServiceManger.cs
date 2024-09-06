using MovieCardAPI.Model.Repository;
using MovieCardAPI.Model.Utility;

namespace MovieCardAPI.Model.Service;

public class ServiceManager(Lazy<IMovieService> movieService) : IServiceManager
{
    public IMovieService MovieService => movieService.Value;
}
