using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.AspNetCore.Mvc;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Subjects;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly AppDbContext _db;

    public SubjectsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("search/{name}")]
    public async Task<IActionResult> SearchSubject(string name)
    {
        var response = await SubjectFeature.SearchByNameAsync(name, _db);
        if (response.Data == null) return NotFound();
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var response = await SubjectFeature.DeleteAsync(id, _db);
        if (response == null) return NotFound();
        return Ok(response);
    }
}
