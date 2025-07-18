public class EmployeeBenefits
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
        public int BenefitId { get; set; }
    public Benefit Benefit { get; set; } = null!;
    public decimal Cost { get; set; }

}

public class Benefit
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal BaseCost { get; set; }
}