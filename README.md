# Employee Management API
A REST API built in .NET to manage employees and their benefits

## Objective:
Build a secure, documented Web API using ASP.NET Core 8, Entity Framework Core with SQLite, and JWT Authentication. The API manages employee records and their associated benefits.

## 🗂️ Project Structure
Use this folder structure:
EmployeeManagementApi/ <br> 
│ <br> 
├── Controllers/ <br> 
│ └── EmployeesController.cs <br> 
│ <br> 
├── Data/ <br> 
│ └── AppDbContext.cs <br> 
│<br> 
├── Dtos/<br> 
│ ├── EmployeeDto.cs <br> 
│ ├── CreateEmployeeDto.cs <br> 
│ ├── UpdateEmployeeDto.cs <br> 
│ └── BenefitDto.cs <br> 
│ <br> 
├── Models/ <br> 
│ ├── Employee.cs <br> 
│ ├── EmployeeBenefit.cs <br> 
│ └── BenefitType.cs<br> 
│<br> 
├── Services/<br> 
│ └── TokenService.cs <br> 
│<br> 
├── Auth/<br> 
│ └── JwtSettings.cs <br> 
│<br> 
├── Migrations/ <br> 
│ └── (EF Migrations will go here) <br> 
│<br> 
├── Tests/ <br> 
│ └── EmployeesControllerTests.cs <br> 
│<br> 
├── appsettings.json <br> 
├── Program.cs <br> 
└── EmployeeManagementApi.csproj <br> 

## ✅ Requirements & Tasks
### 1. Set Up the Project
Use .NET 8 SDK
Scaffold with 
```dotnet new webapi -n EmployeeManagementApi```
Add EF Core packages:
```
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

### 2. Model & Database Design
Refactor Employee and EmployeeBenefits into a relational structure:
Employee.cs
EmployeeBenefit.cs
BenefitType.cs (enum)
Use EF Core with DbContext in AppDbContext.cs.
 Create an SQLite database.
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 3. API Endpoints

Create an `EmployeesController` with the following RESTful routes:

| HTTP Method | Route                            | Description                  |
|-------------|----------------------------------|------------------------------|
| GET         | `/api/employees`                | Get all employees            |
| GET         | `/api/employees/{id}`           | Get employee by ID           |
| POST        | `/api/employees`                | Create employee              |
| PUT         | `/api/employees/{id}`           | Update employee              |
| DELETE      | `/api/employees/{id}`           | Delete employee              |
| GET         | `/api/employees/{id}/benefits`  | Get benefits for an employee |

> 🔸 **Note:** Use **DTOs** to avoid exposing internal models.


### 4. Authentication & Authorization
Implement JWT authentication:
Secure all routes.
Allow only authenticated users to access the API.

Requirements:
Add a login endpoint that returns a JWT.
Use symmetric signing key in appsettings.json.
Add TokenService.cs for token generation.

### 5. Swagger Documentation
Enable Swagger UI in Program.cs.
Use [ProducesResponseType], [HttpGet("...")], and XML comments to document endpoints.

### 6. Testing
Write unit tests for each endpoint using xUnit and Microsoft.AspNetCore.Mvc.Testing.
Directory: Tests/EmployeesControllerTests.cs

Test cases:
Should return 200 OK for GetAllEmployees
Should return 404 for GetEmployeeById when not found
Should return 201 for successful CreateEmployee
Should return 401 when accessing endpoints without JWT

## 🧠 Bonus Task
🛠 Refactor Employee & Benefits
``` csharp
Move benefits to a separate table:
public class Employee
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? SocialSecurityNumber { get; set; }

    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    public List<EmployeeBenefit> Benefits { get; set; } = new();
}

public class EmployeeBenefit
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public BenefitType BenefitType { get; set; }
    public decimal Cost { get; set; }

    public Employee Employee { get; set; } = null!;
}

public enum BenefitType
{
    Health,
    Dental,
    Vision
}
```

### 🔐 Use DbContext (No Repositories)
Query AppDbContext directly in your controller actions.
Rationale: Repositories can unnecessarily abstract simple use-cases and leak EF semantics.
This aligns with real-world preferences (e.g., at startups, lean teams, or microservices).

### 🔚 Submission Checklist
 - Project builds and runs locally via dotnet run
 - All routes tested and functional
 - JWT Auth works and restricts endpoints
 - Swagger UI is enabled and documented
 - Benefits are in separate table
 - At least 1 test per endpoint exists in Tests/
 - Codebase uses consistent naming and folder structure
