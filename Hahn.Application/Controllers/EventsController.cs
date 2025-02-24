using Hahn.Application.Commands.Events;
using Hahn.Application.Commands.Recipies;
using Hahn.Application.Queries.Events;
using Hahn.Application.Queries.Recipies;
using Hahn.Data.Dtos.Events;
using Hahn.Data.Dtos.Recipies;
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
        var query = new GetAllEventsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchEventsByTitle([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("Title parameter is required.");
        }

        var query = new SearchEventsByTitleQuery(title);
        IEnumerable<EventsDto> events = await _mediator.Send(query);

        return Ok(events);
    }


    [HttpPost("upsert")]
    [SwaggerOperation(Summary = "Creates or updates a event.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The created or updated event.", typeof(EventsDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid events data.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
    public async Task<ActionResult<EventsDto>> Upsert([FromBody] UpsertEventDto dto)
    {
        var command = new UpsertEventCommand(dto);
        var events = await _mediator.Send(command);
        return Ok(events);
    }


    [HttpDelete("delete/{id}")]
    [SwaggerOperation(Summary = "Deletes a recipe by its ID.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Recipe deleted successfully.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteEventCommand(id);
        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound($"Recipe with ID {id} not found.");
        }
        return NoContent();
    }

}