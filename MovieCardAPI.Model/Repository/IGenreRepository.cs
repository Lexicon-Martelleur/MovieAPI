﻿using MovieCardAPI.Entities;

namespace MovieCardAPI.Model.Repository;

public interface IGenreRepository : IBaseRepository<Genre>
{
    Task<bool> IsExistingGenres(IEnumerable<int> ids);

    Task<IEnumerable<Genre>> GetGenres(int movieId);

    Task<IEnumerable<Genre>> GetAllGenres();
}