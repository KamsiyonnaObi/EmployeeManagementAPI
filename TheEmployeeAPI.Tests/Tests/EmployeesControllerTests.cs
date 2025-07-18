using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TheEmployeeAPI.Tests
{
    public class EmployeeControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly int _employeeId = 1;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public EmployeeControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllEmployees_ReturnsOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Employees");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsOkResult()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Employees/1");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetEmployeeById_ReturnsNotFoundResult()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/employees/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateEmployee_ReturnsCreatedResult()
        {
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/employees", new Employee { FirstName = "John", LastName = "Doe" });

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNoContentResult()
        {
            // Arrange
            var client = _factory.CreateClient();

            var newEmployee = new Employee { FirstName = "Kamsi", LastName = "Obi" };
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Employees.Add(newEmployee);
                await db.SaveChangesAsync();
            }

            // Act
            var response = await client.DeleteAsync($"/employees/{newEmployee.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNotFoundResult()
        {
            var client = _factory.CreateClient();
            var response = await client.DeleteAsync("/employees/999999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetBenefitsForEmployee_ReturnsOkResult()
        {
            // Act
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"/employees/{_employeeId}/benefits");

            // Assert
            response.EnsureSuccessStatusCode();

            // var benefits = await response.Content.ReadFromJsonAsync<IEnumerable<GetEmployeeBenefitsDto>>();
            // Assert.Equal(2, benefits.Count());
        }
    
    }
}