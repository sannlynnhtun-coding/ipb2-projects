using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.AspNetCore.Mvc;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Results;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ResultsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetResults([FromQuery] GetResultsRequest request)
    {
        var response = await ResultFeature.GetListAsync(request, _db);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetResult(int id)
    {
        var result = await ResultFeature.GetByIdAsync(id, _db);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResult(CreateResultRequest request)
    {
        var response = await ResultFeature.CreateAsync(request, _db);
        return CreatedAtAction(nameof(GetResult), new { id = response.Id }, response);
    }
}
