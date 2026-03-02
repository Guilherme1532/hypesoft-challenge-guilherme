using Hypesoft.Application.Commands.Categories;
using Hypesoft.Application.Queries.Categories;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hypesoft.API.Controllers;

[ApiController]
[Route("api/categories")]

public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> Create(CreateCategoryCommand command, CancellationToken ct)
    {
        var created = await _mediator.Send(command, ct);
        return Ok(created);
    }

    [HttpGet]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new ListCategoriesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "UserOnly")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id), ct);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCategoryCommand body, CancellationToken ct)
    {
        var ok = await _mediator.Send(body with { Id = id }, ct);
        if (!ok)
        {
            return NotFound();
        }
        return Ok();
    }
    
    [HttpDelete("{id}")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        var ok = await _mediator.Send(new DeleteCategoryCommand(id), ct);
        if (!ok)
        {
            return NotFound();
        }
        return NoContent();
    }
}