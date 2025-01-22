using Hahn.Application.Commands.Recipies;
using Hahn.Application.Queries.Recipies;
using Hahn.Data.Dtos.Recipies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;

namespace Hahn.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipiesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RecipiesController> _logger;

        public RecipiesController(IMediator mediator, IMemoryCache cache, ILogger<RecipiesController> logger)
        {
            _mediator = mediator;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all Recipies.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of all Recipies.", typeof(IEnumerable<FoodRecipeDto>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var query = new GetAllRecipiesQuery();
                var recipes = await _mediator.Send(query);

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
              
                return Ok(recipes);
            }
            catch (TimeoutException ex)
            {
                _logger.LogError(ex, "Timeout occurred while fetching all recipes.");
                return StatusCode(504, "The request timed out while fetching recipes. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching all recipes.");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieves a single recipe by its ID.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The requested recipe.", typeof(FoodRecipeDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<FoodRecipeDto>> GetById(Guid id)
        {
            var query = new GetRecipeByIdQuery(id);
            var recipe = await _mediator.Send(query);
            if (recipe == null)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }
            return Ok(recipe);
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchRecipiesByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Title parameter is required.");
            }

            var query = new SearchRecipiesByTitleQuery(title);
            IEnumerable<FoodRecipeDto> recipies = await _mediator.Send(query);

            return Ok(recipies);
        }


        [HttpPost("upsert")]
        [SwaggerOperation(Summary = "Creates or updates a recipe.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The created or updated recipe.", typeof(FoodRecipeDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid recipe data.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<FoodRecipeDto>> Upsert([FromBody] UpsertFoodRecipeDto dto)
        {
            var command = new UpsertFoodRecipeCommand(dto);
            var recipe = await _mediator.Send(command);
            return Ok(recipe);
        }

       
        [HttpDelete("delete/{id}")]
        [SwaggerOperation(Summary = "Deletes a recipe by its ID.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Recipe deleted successfully.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Recipe not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteFoodRecipeCommand(id);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound($"Recipe with ID {id} not found.");
            }
            return NoContent();
        }
    }
}
