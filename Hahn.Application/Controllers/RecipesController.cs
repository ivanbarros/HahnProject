using Hahn.Application.Commands.Recipies;
using Hahn.Application.Queries.Recipies;
using Hahn.Data.Dtos.Recipies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// GET: api/recipes
        /// Retrieves all recipes.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<FoodRecipeDto>> GetAll()
        {
            var query = new GetAllRecipesQuery();
            return await _mediator.Send(query);
        }

        /// <summary>
        /// GET: api/recipes/{id}
        /// Retrieves a single recipe by its ID.
        /// </summary>
        [HttpGet("{id}")]
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
        /// POST: api/recipes/upsert
        /// Creates or updates a recipe.
        /// </summary>
        [HttpPost("upsert")]
        public async Task<ActionResult<FoodRecipeDto>> Upsert([FromBody] UpsertFoodRecipeDto dto)
        {
            var command = new UpsertFoodRecipeCommand(dto);
            var recipe = await _mediator.Send(command);
            return Ok(recipe);
        }

        /// <summary>
        /// DELETE: api/recipes/delete/{id}
        /// Deletes a recipe by its ID.
        /// </summary>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteFoodRecipeCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
