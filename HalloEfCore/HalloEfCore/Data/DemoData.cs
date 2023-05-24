using Bogus;
using HalloEfCore.Model;

namespace HalloEfCore.Data
{
    public static class DemoData
    {

        public static IEnumerable<Employee> GetDemoEmployees(int amount = 100, IEnumerable<Customer>? customers = null, IEnumerable<Department>? departments = null)
        {
            var faker = new Faker<Employee>("de");
            faker.UseSeed(69);
            faker.RuleFor(x => x.Name, x => x.Name.FullName());
            faker.RuleFor(x => x.Salary, x => x.Finance.Amount() * 3);
            if (customers != null && customers.Count() > 0)
                faker.RuleFor(x => x.Customers, x => x.PickRandom(customers, x.Random.Number(5)).ToList());

            if (departments != null && departments.Count() > 0)
                faker.RuleFor(x => x.Departments, x => x.PickRandom(departments, x.Random.Number(1, 3)).ToList());

            return faker.Generate(amount);
        }

        public static IEnumerable<Department> GetDemoDepartments(int amount = 10)
        {
            var faker = new Faker<Department>("de");
            faker.UseSeed(69);
            faker.RuleFor(x => x.Name, x => x.Commerce.Department());
            return faker.Generate(amount);
        }

        public static IEnumerable<Customer> GetDemoCustomers(int amount = 100)
        {
            var faker = new Faker<Customer>("de");
            faker.UseSeed(69);
            faker.RuleFor(x => x.Name, x => x.Name.FullName());
            faker.RuleFor(x => x.Address, x => x.Address.FullAddress());

            return faker.Generate(amount);
        }

    }
}
