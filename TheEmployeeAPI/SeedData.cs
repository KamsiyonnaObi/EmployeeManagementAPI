

public static class SeedData
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        if (!context.Benefits.Any())
        {
            var baseBenefits = new List<Benefit>
            {
                new Benefit { Name = "Health Insurance", Description = "Covers medical expenses", BaseCost = 150 },
                new Benefit { Name = "Dental Plan", Description = "Includes dental check-ups and procedures", BaseCost = 75 },
                new Benefit { Name = "Vision Care", Description = "Covers eye exams and eyewear", BaseCost = 50 },
                new Benefit { Name = "Life Insurance", Description = "Life insurance coverage", BaseCost = 100 }
            };

            context.Benefits.AddRange(baseBenefits);
            context.SaveChanges();
        }
        
        if (!context.Employees.Any())
        {
            var firstNames = new[] { "John", "Jane", "Alex", "Taylor", "Chris", "Morgan", "Casey", "Jordan", "Cameron", "Riley" };
            var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Martinez", "Lee" };
            var streets = new[] { "Main St", "Elm St", "Oak St", "Pine Ave", "Maple Rd", "Cedar Blvd" };
            var cities = new[] { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix" };
            var states = new[] { "NY", "CA", "IL", "TX", "AZ" };

            var random = new Random();

            var employees = Enumerable.Range(1, 10).Select(i =>
            {
                var benefits = context.Benefits.ToList();
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";

                var employee = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    SocialSecurityNumber = $"{random.Next(100, 999)}-{random.Next(10, 99)}-{random.Next(1000, 9999)}",
                    Salary = random.Next(40000, 120000),
                    Address1 = $"{random.Next(100, 999)} {streets[random.Next(streets.Length)]}",
                    City = cities[random.Next(cities.Length)],
                    State = states[random.Next(states.Length)],
                    ZipCode = $"{random.Next(10000, 99999)}",
                    PhoneNumber = $"555-{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    Email = email,
                };

                var selectedBenefits = benefits.OrderBy(_ => random.Next()).Take(random.Next(1, 4)).ToList();
                employee.Benefits = selectedBenefits.Select(b => new EmployeeBenefits
                {
                    Benefit = b,
                    BenefitId = b.Id,
                    Cost = Math.Round(b.BaseCost + (decimal)(random.NextDouble() * 50), 2) // Add variability
                }).ToList();
                
                return employee;

            }).ToList();

            context.Employees.AddRange(employees);
            context.SaveChanges();
        }

    }
}