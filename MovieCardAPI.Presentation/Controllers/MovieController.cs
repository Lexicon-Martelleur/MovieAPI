using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Service;
using MovieCardAPI.Presentation.Constants;
using MovieCardAPI.Presentation.Error;
using System.Net;
using System.Text.Json;

namespace MovieCardAPI.Presentation.Controllers;

[ApiController]
[Route(AppRouter.MOVIE)]
[Authorize]
public class MovieController : ControllerBase
{
    private readonly ILogger<MovieController> _logger;
    private readonly IServiceManager _service;

    public MovieController(ILogger<MovieController> logger, IServiceManager service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies(
        [FromQuery] PaginationDTO query)
    {
        var (Movies, Pagination) = await _service.MovieService.GetMovies(query);
        Response.Headers.Append(
            CustomHeader.Pagination,
            JsonSerializer.Serialize(Pagination)
        );
        return Ok(Movies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDTO>> GetMovie(
        [FromRoute] int id
    )
    {
        var movieDTO = await _service.MovieService.GetMovie(id);
        return Ok(movieDTO);
    }

    [HttpGet("{id}/details")]
    public async Task<ActionResult<MovieDetailsDTO>> GetMovieDetails(
        [FromRoute] int id
    )
    {
        var movieDetailsDTO = await _service.MovieService.GetMovieDetails(id);
        return Ok(movieDetailsDTO);
    }

    [HttpPost(Name = "CreateMovie")]
    public async Task<ActionResult<MovieDTO>> CreateMovie(
        [FromBody] MovieForCreationDTO movie)
    {
        var createdMovie = await _service.MovieService.CreateMovie(movie);
        return CreatedAtRoute(nameof(CreateMovie), createdMovie);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MovieDTO>> UpdateMovie(
        [FromRoute] int id,
        [FromBody] MovieForUpdateDTO movie)
    {
        var updatedMovie = await _service.MovieService.UpdateMovie(id, movie);
        return Ok(updatedMovie);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(
        [FromRoute] int id)
    {
        var isDeleted = await _service.MovieService.DeleteMovie(id);
        if (!isDeleted)
        {
            return NotFound(new ApiError(
               HttpStatusCode.NotFound,
               "Failed to delete the movie. Please check the provided data and try again."
           ));
        }
        return Ok();
    }
}
