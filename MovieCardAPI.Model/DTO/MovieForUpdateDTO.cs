﻿using MovieCardAPI.Model.Validation;
using MovieCardAPI.Model.ValueObjects;

namespace MovieCardAPI.Model.DTO;

public record class MovieForUpdateDTO : IMovie {
    [ValidateMovieTitle]
    public required string Title { get; set; }
    
    [ValidateMovieRating]
    public required int Rating { get; set; }

    [UNIXTimestampValidation]
    public required long TimeStamp { get; set; }

    [ValidateMovieDescription]
    public required string Description { get; set; }

    public required int DirectorId { get; set; }

    public required int[] ActorIds { get; set; }

    public required int[] GenreIds { get; set; }
};
