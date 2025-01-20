using Hahn.Application.Commands.Recipies;
using Hahn.Application.Queries.Recipies;
using Hahn.Data.Dtos.Recipies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Hahn.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all recipes.
        /// </summary>
        /// <returns>List of all recipes.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves all recipes.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of all recipes.", typeof(IEnumerable<FoodRecipeDto>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IEnumerable<FoodRecipeDto>> GetAll()
        {
            var query = new GetAllRecipesQuery();
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Retrieves a single recipe by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe.</param>
        /// <returns>The requested recipe.</returns>
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

        /// <summary>
        /// Creates or updates a recipe.
        /// </summary>
        /// <param name="dto">The recipe data transfer object.</param>
        /// <returns>The created or updated recipe.</returns>
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

        /// <summary>
        /// Searches for recipes by title.
        /// </summary>
        /// <param name="title">The title of the recipe to search for.</param>
        /// <returns>List of recipes matching the title.</returns>
        [HttpGet("search")]
        [SwaggerOperation(Summary = "Searches for recipes by title.")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of matching recipes.", typeof(IEnumerable<FoodRecipeDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Title parameter is required.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "No recipes found matching the provided title.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<IEnumerable<FoodRecipeDto>>> SearchByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("Title parameter is required.");
            }

            var query = new SearchRecipesByTitleQuery(title);
            var recipes = await _mediator.Send(query);
            if (recipes == null || !recipes.Any())
            {
                return NotFound("No recipes found matching the provided title.");
            }

            return Ok(recipes);
        }

        /// <summary>
        /// Deletes a recipe by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the recipe to delete.</param>
        /// <returns>No content if deletion is successful.</returns>
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
