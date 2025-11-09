using Xunit;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContosoUniversity.Tests.Services
{
    public class StudentServiceTests
    {
        private SchoolContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;

            var context = new SchoolContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Students.AddRange(
                new Student { ID = 1, FirstName = "Thomas", LastName = "Cat" },
                new Student { ID = 2, FirstName = "Gerald", LastName = "Mouse" }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GetStudentCountAsync_ReturnsCorrectNumberOfStudents()
        {
            var context = GetInMemoryContext();
            var service = new StudentService(context);

            var result = await service.GetStudentCountAsync();

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ReturnsStudent_WhenExists()
        {
            var context = GetInMemoryContext();
            var service = new StudentService(context);

            var student = await service.GetStudentByIdAsync(1);

            Assert.NotNull(student);
            Assert.Equal("Thomas", student.FirstName);
        }
    }
}
