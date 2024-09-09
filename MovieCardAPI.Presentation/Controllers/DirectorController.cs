using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieCardAPI.Model.DTO;
using MovieCardAPI.Model.Service;
using MovieCardAPI.Presentation.Constants;

namespace MovieCardAPI.Presentation.Controllers;

[ApiController]
[Route(Router.DIRECTOR)]
public class DirectorController : ControllerBase
{
    private readonly ILogger<ActorController> _logger;
    private readonly IServiceManager _service;

    public DirectorController(ILogger<ActorController> logger, IServiceManager service)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DirectorDTO>>> GetDirectors()
    {
        var directorDTOs = await _service.DirectorService.GetDirectors();
        return Ok(directorDTOs);
    }
}
