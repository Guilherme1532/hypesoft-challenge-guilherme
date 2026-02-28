using Hypesoft.Application.Commands.Categories;
using Hypesoft.Application.Queries.Categories;
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
    public async Task<IActionResult> Create(CreateCategoryCommand command, CancellationToken ct)
    {
        var created = await _mediator.Send(command, ct);
        return Ok(created);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new ListCategoriesQuery(), ct);
        return Ok(result);
    }
}