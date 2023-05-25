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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bestellung>().ToTable("Orders");

            modelBuilder.Entity<Bestellung>().HasOne(x => x.Lieferadresse)
                                             .WithMany(x => x.BestellungenAlsLieferadresse);

            modelBuilder.Entity<Bestellung>().HasOne(x => x.Rechnungsadresse)
                                             .WithMany(x => x.BestellungenAlsRechnungsadresse);

            modelBuilder.Entity<Produkt>().HasMany(x => x.Lieferanten)
                                          .WithMany(x => x.Produkte)
                                          .UsingEntity("LPs",
                                                 l => l.HasOne(typeof(Lieferant)).WithMany().HasForeignKey("LId"),
                                                 r => r.HasOne(typeof(Produkt)).WithMany().HasForeignKey("Pid"));


            //modelBuilder.Entity<Adresse>().HasMany(x => x.BestellungenAlsLieferadresse)
            //                              .WithOne(x => x.Lieferadresse);

            //modelBuilder.Entity<Adresse>().HasMany(x => x.BestellungenAlsRechnungsadresse)
            //                              .WithOne(x => x.Rechnungsadresse);

        }
    }
}