using IBP2.StudentResultManagementSystemMVC.Models;
using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace IBP2.StudentResultManagementSystemMVC.Services.Subject
{
    public class SubjectService : ISubjectService
    {
        private readonly AppDbContext _context;

        public SubjectService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateSubjectResponse> CreateSubjectAsync(CreateSubjectRequest request)
        {
            var subject = new IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels.Subject
            {
                Name = request.Name,
                Credits = request.Credits,
                IsDelete = false
            };

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return new CreateSubjectResponse
            {
                Id = subject.Id,
                Name = subject.Name,
                Credits = subject.Credits,
                IsDelete = subject.IsDelete ?? false
            };
        }

        public async Task<GetSubjectResponse?> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);

            if (subject == null) return null;

            return new GetSubjectResponse
            {
                Data = new SubjectModel
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    Credits = subject.Credits,
                    IsDelete = subject.IsDelete ?? false
                }
            };
        }

        public async Task<GetSubjectsResponse> GetSubjectsAsync()
        {
            var subjects = await _context.Subjects
                .AsNoTracking()
                .Where(x => x.IsDelete == false)
                .Select(subject => new SubjectModel
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    Credits = subject.Credits,
                    IsDelete = subject.IsDelete ?? false
                })
                .ToListAsync();

            return new GetSubjectsResponse
            {
                Data = subjects
            };
        }

        public async Task<UpdateSubjectResponse?> UpdateSubjectAsync(UpdateSubjectRequest request)
        {
            var subject = await _context.Subjects
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDelete == false);

            if (subject == null) return null;

            subject.Name = request.Name;
            subject.Credits = request.Credits;

            await _context.SaveChangesAsync();

            return new UpdateSubjectResponse
            {
                Id = subject.Id,
                Name = subject.Name,
                Credits = subject.Credits,
                IsDelete = subject.IsDelete ?? false
            };
        }

        public async Task<bool> DeleteSubjectAsync(int id)
        {
            var subject = await _context.Subjects
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == false);

            if (subject == null) return false;

            subject.IsDelete = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
