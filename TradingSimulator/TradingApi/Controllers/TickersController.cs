using Microsoft.AspNetCore.Mvc;
using TradingApi.Services.Interfaces;

[ApiController]
[Route("api/tickers")]
public class TickersController : ControllerBase
{
    private readonly IPolygonService _polygonService;

    public TickersController(IPolygonService polygonService)
    {
        _polygonService = polygonService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Query is required");

        var results = await _polygonService.SearchTickersAsync(q);

        return Ok(results);
    }
}