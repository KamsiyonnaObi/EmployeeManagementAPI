using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]

public class EmployeesController(AppDbContext dbContext, ILogger<EmployeesController> logger) : BaseController
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly ILogger<EmployeesController> _logger = logger;
    /// <summary>
    /// Get all employees.
    /// </summary>
    /// <returns>An array of all employees.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllEmployees()
    {
        try
        {
            var employees = await _dbContext.Employees.ToArrayAsync();
            _logger.LogInformation("Employees successfully returned");

            return Ok(employees.Select(EmployeeToGetEmployeeDto));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching employees");
            return StatusCode(500, "An error occurred while fetching employees");
        }
    }

    /// <summary>
    /// Get an employee by their id.
    /// </summary>
    /// <param name="id">The ID of the employee.</param>
    /// <returns>A single employee.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        try
        {
            var employee = await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(EmployeeToGetEmployeeDto(employee));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching employee with id: {employeeId}", id);
            return StatusCode(500, "An error occurred while fetching employee");
        }
    }

    /// <summary>
    /// Creates a new employee.
    /// </summary>
    /// <param name="employeeRequest">The employee to be created.</param>
    /// <returns>A link to the employee that was created.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto employeeRequest)
    {
        var newEmployee = new Employee
        {
            FirstName = employeeRequest.FirstName!,
            LastName = employeeRequest.LastName!,
            SocialSecurityNumber = employeeRequest.SocialSecurityNumber,
            Address1 = employeeRequest.Address1,
            Address2 = employeeRequest.Address2,
            City = employeeRequest.City,
            State = employeeRequest.State,
            ZipCode = employeeRequest.ZipCode,
            PhoneNumber = employeeRequest.PhoneNumber,
            Email = employeeRequest.Email
        };

        _dbContext.Employees.Add(newEmployee);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, newEmployee);
    }

    [HttpPut]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto UpdateEmployeeRequest, int id)
    {
        var existingEmployee = await _dbContext.Employees.FindAsync(id);
        if (existingEmployee == null)
        {
            _logger.LogWarning("Employee with ID: {EmployeeId} not found", id);
            return NotFound();
        }

        _logger.LogDebug("Updating employee details for ID: {EmployeeId}", id);
        existingEmployee.Address1 = UpdateEmployeeRequest.Address1;
        existingEmployee.Address2 = UpdateEmployeeRequest.Address2;
        existingEmployee.City = UpdateEmployeeRequest.City;
        existingEmployee.State = UpdateEmployeeRequest.State;
        existingEmployee.Salary = UpdateEmployeeRequest.Salary;
        existingEmployee.ZipCode = UpdateEmployeeRequest.ZipCode;
        existingEmployee.PhoneNumber = UpdateEmployeeRequest.PhoneNumber;
        existingEmployee.Email = UpdateEmployeeRequest.Email;

        try
        {
            _dbContext.Entry(existingEmployee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Employee with ID: {EmployeeId} successfully updated", id);
            return Ok(existingEmployee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating employee with ID: {EmployeeId}", id);
            return StatusCode(500, "An error occurred while updating the employee");
        }
    }
    /// <summary>
    /// Get an employee by their id, then delete.
    /// </summary>
    /// <param name="id">The ID of the employee.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
    
    private static EmployeeDto EmployeeToGetEmployeeDto(Employee employee)
    {
        return new EmployeeDto
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Address1 = employee.Address1,
            Address2 = employee.Address2,
            City = employee.City,
            State = employee.State,
            ZipCode = employee.ZipCode,
            PhoneNumber = employee.PhoneNumber,
            Email = employee.Email
        };
    }
}