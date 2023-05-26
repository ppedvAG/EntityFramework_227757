using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ppedv.ByteBay.Model;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http.Headers;

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

        public override int SaveChanges()
        {

            foreach (var item in ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted))
            {
                if (item.Entity is Entity entity)
                {
                    entity.IsDeleted = true;
                    item.State = EntityState.Modified;

                    if (entity is Bestellung best)
                    {
                        foreach (var pos in best.Positionen)
                        {
                            pos.IsDeleted = true;
                        }
                    }
                }
            }

            foreach (var item in ChangeTracker.Entries().Where(x => x.State == EntityState.Modified))
            {
                if (item.Entity is Entity entity)
                    entity.Modified = DateTime.Now;
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Entity>().HasQueryFilter(x => !x.IsDeleted);


            Expression<Func<Entity, bool>> filterExpr = entity => !entity.IsDeleted;

            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                // check if current entity type is child of BaseModel
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(Entity)))
                {
                    // modify expression to handle correct child type
                    var parameter = Expression.Parameter(mutableEntityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);

                    // set filter
                    mutableEntityType.SetQueryFilter(lambdaExpression);
                }
            }

            
            //todo für alle Entity machen!
            modelBuilder.Entity<Produkt>().Property(x => x.Modified)
                                         .IsConcurrencyToken();

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

            modelBuilder.Entity<Bestellung>().HasMany(x => x.Positionen)
                                             .WithOne(x => x.Bestellung);
            //.OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Adresse>().HasMany(x => x.BestellungenAlsLieferadresse)
            //                              .WithOne(x => x.Lieferadresse);

            //modelBuilder.Entity<Adresse>().HasMany(x => x.BestellungenAlsRechnungsadresse)
            //                              .WithOne(x => x.Rechnungsadresse);

        }
    }
}