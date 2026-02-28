using Hypesoft.Application.Commands.Products;
using Hypesoft.Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hypesoft.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken ct)
    {
        var created = await _mediator.Send(command, ct);
        return Ok(created);
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? categoryId = null,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new ListProductsQuery(page, pageSize, categoryId, search),
            ct);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id,
        [FromBody] UpdateProductCommand body,
        CancellationToken ct)
    {
        var command = body with { Id = id };

        var updated = await _mediator.Send(command, ct);

        if (!updated)
            return NotFound();

        return NoContent();
    }
}