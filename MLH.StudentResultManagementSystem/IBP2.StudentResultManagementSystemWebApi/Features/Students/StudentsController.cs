using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.AspNetCore.Mvc;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Students;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public StudentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var response = await StudentFeature.GetListAsync(_db);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var response = await StudentFeature.GetByIdAsync(id, _db);
        if (response == null) return NotFound();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(CreateStudentRequest request)
    {
        var response = await StudentFeature.CreateAsync(request, _db);
        return CreatedAtAction(nameof(GetStudent), new { id = response.Id }, response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var success = await StudentFeature.DeleteAsync(id, _db);
        if (!success) return NotFound();
        return Ok("Student soft-deleted.");
    }
}
