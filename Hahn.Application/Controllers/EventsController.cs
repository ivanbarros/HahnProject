using Hahn.Data.Dtos.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;

namespace Hahn.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IMediator mediator, IMemoryCache cache, ILogger<EventsController> logger)
    {
        _mediator = mediator;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Retrieves all Events.")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of all Events.", typeof(IEnumerable<EventsDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

}