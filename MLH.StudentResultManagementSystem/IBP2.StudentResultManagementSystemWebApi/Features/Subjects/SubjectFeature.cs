using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace IBP2.StudentResultManagementSystemWebApi.Features.Subjects;

public static class SubjectFeature
{
    public static async Task<SearchSubjectResponse> SearchByNameAsync(string name, AppDbContext db)
    {
        var subject = await db.Subjects
            .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower() && !s.IsDelete);

        return new SearchSubjectResponse { Data = subject };
    }

    public static async Task<DeleteSubjectResponse?> DeleteAsync(int id, AppDbContext db)
    {
        var subject = await db.Subjects.FindAsync(id);
        if (subject == null || subject.IsDelete) return null;

        subject.IsDelete = true;
        await db.SaveChangesAsync();

        return new DeleteSubjectResponse { Message = "Subject soft-deleted." };
    }
}
