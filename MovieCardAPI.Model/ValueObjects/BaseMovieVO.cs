using MovieCardAPI.Model.Validation;

namespace MovieCardAPI.Model.ValueObjects;

public interface IMovie {
    string Title { get; set; }
    int Rating { get; set; }
    long TimeStamp { get; set; }
    string Description { get; set; }
};
