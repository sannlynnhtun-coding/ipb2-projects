using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Results;

public static class ResultFeature
{
    public static async Task<GetResultsResponse> GetListAsync(GetResultsRequest request, AppDbContext db)
    {
        var query = db.Results
            .Include(r => r.Student)
            .Include(r => r.Subject)
            .AsNoTracking();

        var totalRecords = await query.CountAsync();
        var data = await query
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new GetResultsResponse { TotalCount = totalRecords, Data = data };
    }

    public static async Task<CreateResultResponse> CreateAsync(CreateResultRequest request, AppDbContext db)
    {
        var grade = request.Marks switch
        {
            >= 90 => "A+",
            >= 80 => "A",
            >= 70 => "B",
            >= 60 => "C",
            >= 50 => "D",
            _ => "F"
        };

        var result = new Result
        {
            StudentId = request.StudentId,
            SubjectId = request.SubjectId,
            Marks = request.Marks,
            Grade = grade
        };

        db.Results.Add(result);
        await db.SaveChangesAsync();

        return new CreateResultResponse
        {
            Id = result.Id,
            StudentId = result.StudentId,
            SubjectId = result.SubjectId,
            Marks = result.Marks,
            Grade = result.Grade
        };
    }

    public static async Task<Result?> GetByIdAsync(int id, AppDbContext db)
    {
        return await db.Results
            .Include(r => r.Student)
            .Include(r => r.Subject)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
