using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hypesoft.API.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet("mongo")]
    public async Task<IActionResult> Mongo([FromServices] IMongoDatabase db)
    {
        var result = await db.RunCommandAsync<BsonDocument>(new BsonDocument("ping", 1));
        return Ok(new { ok = true, database = db.DatabaseNamespace.DatabaseName });
    }
}