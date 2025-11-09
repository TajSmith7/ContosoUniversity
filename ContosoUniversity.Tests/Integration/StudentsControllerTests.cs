using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net;

namespace ContosoUniversity.Tests.Integration
{
    public class StudentsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StudentsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Students_Index_ReturnsSuccessAndContainsText()
        {
            
            var response = await _client.GetAsync("/Students");
           
            response.EnsureSuccessStatusCode(); 

            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("Students", html); // Check students landing page
        }

        [Fact]
        public async Task Get_Students_Details_ReturnsStudent()
        {
            var response = await _client.GetAsync("/Students/Details?id=1");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("Carson", html); // Check student 1 details
        }

        [Fact]
        public async Task Get_Courses_Index_ReturnsSuccessAndContainsText()
        {
            var response = await _client.GetAsync("/Courses");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("Courses", html); //Check courses landing page
        }

        [Fact]
        public async Task Get_Courses_Details_ReturnsCourse()
        {
            var response = await _client.GetAsync("/Courses/Details?id=1045");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains("Calculus", html); // Check course 1 page
        }
    }
}
