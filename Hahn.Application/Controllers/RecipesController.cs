using Hahn.Application.Commands.Recipies;
using Hahn.Application.Queries.Recipies;
using Hahn.Data.Dtos.Recipies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hahn.Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecipesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<FoodRecipeDto>> GetAll()
    {
        var query = new GetAllRecipesQuery();
        return await _mediator.Send(query);
    }

    [HttpPost]
    public async Task<FoodRecipeDto> Create([FromBody] CreateFoodRecipeDto dto)
    {
        var command = new CreateFoodRecipeCommand(dto);
        return await _mediator.Send(command);
    }
}