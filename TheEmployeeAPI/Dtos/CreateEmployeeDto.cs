public class CreateEmployeeDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? SocialSecurityNumber { get; set; }
    public int? Salary { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}