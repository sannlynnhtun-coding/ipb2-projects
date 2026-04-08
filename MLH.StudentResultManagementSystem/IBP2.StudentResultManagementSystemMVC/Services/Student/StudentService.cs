using IBP2.StudentResultManagementSystemMVC.Models;
using IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace IBP2.StudentResultManagementSystemMVC.Services.Student
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CreateStudentResponse> CreateStudentAsync(CreateStudentRequest request)
        {
            var student = new IBP2.StudentResultManagementSystemWebApi.Database.AppDbContextModels.Student
            {
                Name = request.Name,
                Email = request.Email,
                IsDelete = false
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return new CreateStudentResponse
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                IsDelete = student.IsDelete ?? false
            };
        }

        public async Task<GetStudentResponse?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && (x.IsDelete == null || x.IsDelete == false));

            if (student == null) return null;

            return new GetStudentResponse
            {
                Data = new StudentModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    IsDelete = student.IsDelete ?? false
                }
            };
        }

        public async Task<GetStudentsResponse> GetStudentsAsync()
        {
            var students = await _context.Students
                .AsNoTracking()
                .Where(x => x.IsDelete == null || x.IsDelete == false)
                .Select(student => new StudentModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    IsDelete = student.IsDelete ?? false
                })
                .ToListAsync();

            return new GetStudentsResponse
            {
                Data = students
            };
        }

        public async Task<UpdateStudentResponse?> UpdateStudentAsync(UpdateStudentRequest request)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == request.Id && (x.IsDelete == null || x.IsDelete == false));

            if (student == null) return null;

            student.Name = request.Name;
            student.Email = request.Email;

            await _context.SaveChangesAsync();

            return new UpdateStudentResponse
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                IsDelete = student.IsDelete ?? false
            };
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == id && (x.IsDelete == null || x.IsDelete == false));

            if (student == null) return false;

            student.IsDelete = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
