using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Students;

public static class StudentFeature
{
    public static async Task<GetStudentsResponse> GetListAsync(AppDbContext db)
    {
        var students = await db.Students
            .Where(s => s.IsDelete != true)
            .ToListAsync();

        return new GetStudentsResponse { Data = students };
    }

    public static async Task<GetStudentResponse?> GetByIdAsync(int id, AppDbContext db)
    {
        var student = await db.Students
            .FirstOrDefaultAsync(s => s.Id == id && s.IsDelete != true);

        if (student == null) return null;

        return new GetStudentResponse { Data = student };
    }

    public static async Task<CreateStudentResponse> CreateAsync(CreateStudentRequest request, AppDbContext db)
    {
        var student = new Student
        {
            Name = request.Name,
            Email = request.Email,
            IsDelete = false
        };

        db.Students.Add(student);
        await db.SaveChangesAsync();

        return new CreateStudentResponse
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            IsDelete = student.IsDelete
        };
    }

    public static async Task<bool> DeleteAsync(int id, AppDbContext db)
    {
        var student = await db.Students.FindAsync(id);
        if (student == null || student.IsDelete == true) return false;

        student.IsDelete = true;
        await db.SaveChangesAsync();
        return true;
    }
}
