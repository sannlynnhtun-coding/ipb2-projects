using IBP2.StudentResultManagementSystemMVC.Models;
using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace IBP2.StudentResultManagementSystemMVC.Services.Result
{
    public class ResultService : IResultService
    {
        private readonly AppDbContext _context;

        public ResultService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateResultResponse> CreateResultAsync(CreateResultRequest request)
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

            var result = new IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels.Result
            {
                StudentId = request.StudentId,
                SubjectId = request.SubjectId,
                Marks = request.Marks,
                Grade = grade
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return new CreateResultResponse
            {
                Id = result.Id,
                StudentId = result.StudentId,
                SubjectId = result.SubjectId,
                Marks = result.Marks,
                Grade = result.Grade
            };
        }

        public async Task<ResultModel?> GetResultByIdAsync(int id)
        {
            var result = await _context.Results
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (result == null) return null;

            return new ResultModel
            {
                Id = result.Id,
                StudentId = result.StudentId,
                SubjectId = result.SubjectId,
                Marks = result.Marks,
                Grade = result.Grade,
                Student = new StudentModel
                {
                    Id = result.Student.Id,
                    Name = result.Student.Name,
                    Email = result.Student.Email,
                    IsDelete = result.Student.IsDelete
                },
                Subject = new SubjectModel
                {
                    Id = result.Subject.Id,
                    Name = result.Subject.Name,
                    Credits = result.Subject.Credits,
                    IsDelete = result.Subject.IsDelete
                }
            };
        }

        public async Task<GetResultsResponse> GetResultsAsync(int pageNo, int pageSize)
        {
            var query = _context.Results
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var results = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(result => new ResultModel
                {
                    Id = result.Id,
                    StudentId = result.StudentId,
                    SubjectId = result.SubjectId,
                    Marks = result.Marks,
                    Grade = result.Grade,
                    Student = new StudentModel
                    {
                        Id = result.Student.Id,
                        Name = result.Student.Name,
                        Email = result.Student.Email,
                        IsDelete = result.Student.IsDelete
                    },
                    Subject = new SubjectModel
                    {
                        Id = result.Subject.Id,
                        Name = result.Subject.Name,
                        Credits = result.Subject.Credits,
                        IsDelete = result.Subject.IsDelete
                    }
                })
                .ToListAsync();

            return new GetResultsResponse
            {
                TotalCount = totalCount,
                Data = results
            };
        }
    
        public async Task<UpdateResultResponse?> UpdateResultAsync(UpdateResultRequest request)
        {
            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.Id == request.Id);
    
            if (result == null) return null;
    
            var grade = request.Marks switch
            {
                >= 90 => "A+",
                >= 80 => "A",
                >= 70 => "B",
                >= 60 => "C",
                >= 50 => "D",
                _ => "F"
            };
    
            result.StudentId = request.StudentId;
            result.SubjectId = request.SubjectId;
            result.Marks = request.Marks;
            result.Grade = grade;
    
            await _context.SaveChangesAsync();
    
            return new UpdateResultResponse
            {
                Id = result.Id,
                StudentId = result.StudentId,
                SubjectId = result.SubjectId,
                Marks = result.Marks,
                Grade = result.Grade
            };
        }
    }
}
