using HalloEfCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HalloEfCore.Data
{
    public class HalloContext : DbContext
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HalloEfCore;Trusted_Connection=true;";

        public DbSet<Person> Persons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(ConnectionString)
                          .UseLazyLoadingProxies()
                          .LogTo(msg => Debug.WriteLine(msg), LogLevel.Information)
#if DEBUG
                          .EnableSensitiveDataLogging()
                          .EnableDetailedErrors();
#else
            ;
#endif

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Person>().ToTable("Person").UseTpcMappingStrategy();
            //TpCT
            modelBuilder.Entity<Employee>().ToTable("Emplyoees");
            modelBuilder.Entity<Customer>().ToTable("Customers");

        }

    }
}
