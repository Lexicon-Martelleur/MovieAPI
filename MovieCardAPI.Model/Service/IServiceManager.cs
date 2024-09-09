﻿namespace MovieCardAPI.Model.Service;

public interface IServiceManager
{
    IMovieService MovieService { get; }

    IActorService ActorService { get; }

    IDirectorService DirectorService { get; }
}
