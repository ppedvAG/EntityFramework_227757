using Microsoft.EntityFrameworkCore;
using ppedv.ByteBay.Model;
using System.Diagnostics;

namespace ppedv.ByteBay.Data.EfCore
{
    public class ByteBayContext : DbContext
    {
        public DbSet<Adresse> Adressen { get; set; }
        public DbSet<BestellPosition> BestellPositionen { get; set; }
        public DbSet<Bestellung> Bestellung { get; set; }
        public DbSet<Lieferant> Lieferanten { get; set; }
        public DbSet<Produkt> Produkte { get; set; }


        readonly string conString;

        public ByteBayContext(string conString)
        {
            this.conString = conString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(conString)
                          .UseLazyLoadingProxies()
                          .LogTo(x => Debug.WriteLine(x))
                          .EnableSensitiveDataLogging()
                          .EnableDetailedErrors();
        }
    }
}