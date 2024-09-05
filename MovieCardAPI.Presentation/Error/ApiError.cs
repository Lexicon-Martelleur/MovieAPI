using System.Net;

namespace MovieCardAPI.Presentation.Error;

public record class ApiError(
    HttpStatusCode Status,
    string Message,
    string Details = ""
);
