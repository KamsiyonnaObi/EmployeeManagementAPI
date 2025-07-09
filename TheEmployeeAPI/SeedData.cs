

public static class SeedData
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();

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
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var email = $"{firstName.ToLower()}.{lastName.ToLower()}{i}@example.com";

                return new Employee
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
                    Benefits = GenerateRandomBenefits(random)
                };
            }).ToList();

            context.Employees.AddRange(employees);
            context.SaveChanges();
        }

    }
    private static List<EmployeeBenefits> GenerateRandomBenefits(Random random)
    {
        var benefits = new List<EmployeeBenefits>();
        var benefitTypes = Enum.GetValues(typeof(BenefitType)).Cast<BenefitType>().ToList();
        var selectedTypes = benefitTypes.OrderBy(_ => random.Next()).Take(random.Next(1, 4)).ToList();

        foreach (var type in selectedTypes)
        {
            benefits.Add(new EmployeeBenefits
            {
                BenefitType = type,
                Cost = Math.Round((decimal)(random.NextDouble() * 100 + 25), 2) // Cost between 25.00 and 125.00
            });
        }

        return benefits;
    }
}