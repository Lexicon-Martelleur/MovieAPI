namespace MovieCardAPI.Model.Service;

public class ServiceManager(
    Lazy<IMovieService> movieService,
    Lazy<IActorService> actorService,
    Lazy<IDirectorService> directorService
) : IServiceManager
{
    public IMovieService MovieService => movieService.Value;

    public IActorService ActorService => actorService.Value;

    public IDirectorService DirectorService => directorService.Value;
}
