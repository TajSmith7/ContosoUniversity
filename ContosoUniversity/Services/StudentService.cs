using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Services
{
    public interface IStudentService
    {
        Task<int> GetStudentCountAsync();
        Task<Student?> GetStudentByIdAsync(int id);
    }

    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;

        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public async Task<int> GetStudentCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
        }
    }
}
