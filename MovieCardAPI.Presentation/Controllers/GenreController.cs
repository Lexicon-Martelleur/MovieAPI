using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Service;
using MovieCardAPI.Presentation.Constants;


namespace MovieCardAPI.Presentation.Controllers;

[ApiController]
[Route(Router.GENRE)]
public class GenreController: ControllerBase
{
    private readonly ILogger<ActorController> _logger;
    private readonly IServiceManager _service;

    public GenreController(ILogger<ActorController> logger, IServiceManager service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DirectorDTO>>> GetGenres()
    {
        var genreDTOs = await _service.GenreService.GetGenres();
        return Ok(genreDTOs);
    }
}
