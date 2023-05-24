using HalloEfCore.Model;
using Microsoft.EntityFrameworkCore;

namespace HalloEfCore.Data
{
    public class HalloContext : DbContext
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=HalloEfCore;Trusted_Connection=true;";

        public DbSet<Person> Persons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
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
