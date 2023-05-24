﻿using Bogus;
using HalloEfCore.Model;

namespace HalloEfCore.Data
{
    public static class DemoData
    {

        public static IEnumerable<Employee> GetDemoEmployees(int amount = 100, IEnumerable<Customer>? customers = null)
        {
            var faker = new Faker<Employee>("de");
            faker.UseSeed(69);
            faker.RuleFor(x => x.Name, x => x.Name.FullName());
            faker.RuleFor(x => x.Salary, x => x.Finance.Amount() * 3);
            if (customers != null && customers.Count() > 0)
                faker.RuleFor(x => x.Customers, x => x.PickRandom(customers,x.Random.Number( 5)).ToList());

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
