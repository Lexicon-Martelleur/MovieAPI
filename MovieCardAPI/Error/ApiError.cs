using System.Net;

namespace MovieCardAPI.Error;

public record class ApiError(
    HttpStatusCode Status,
    string Message,
    string Details = ""
);
