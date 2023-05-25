using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ppedv.ByteBay.Model;
using System.Reflection;

namespace ppedv.ByteBay.Data.EfCore.Tests
{
    public class ByteBayContextTests
    {
        string conString = "Server=(localdb)\\mssqllocaldb;Database=ByteBay_test;Trusted_Connection=true";

        [Fact]
        public void Can_create_Db()
        {
            var con = new ByteBayContext(conString);
            con.Database.EnsureDeleted();

            var result = con.Database.EnsureCreated();

            Assert.True(result);
        }

        [Fact]
        public void Can_add_Product()
        {
            var prod = new Produkt() { Farbe = "Gelb", Gewicht = 12.60, Name = "Dings" };
            var con = new ByteBayContext(conString);
            con.Database.EnsureCreated();

            con.Add(prod);
            var result = con.SaveChanges();

            Assert.Equal(1, result);
        }

        [Fact]
        public void Can_read_Product()
        {
            var prod = new Produkt() { Farbe = "Gelb", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            using (var con = new ByteBayContext(conString))
            {
                con.Database.EnsureCreated();
                con.Add(prod);
                con.SaveChanges();
            }

            using (var con = new ByteBayContext(conString))
            {
                var loaded = con.Produkte.Find(prod.Id);
                Assert.Equal(prod.Name, loaded.Name);
            }
        }

        [Fact]
        public void Can_update_Product()
        {
            var prod = new Produkt() { Farbe = "Gelb", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            var newName = $"Bums_{Guid.NewGuid()}";
            using (var con = new ByteBayContext(conString))
            {
                con.Database.EnsureCreated();
                con.Add(prod);
                con.SaveChanges();
            }

            using (var con = new ByteBayContext(conString))
            {
                var loaded = con.Produkte.Find(prod.Id);
                loaded.Name = newName;
                con.SaveChanges();
            }

            using (var con = new ByteBayContext(conString))
            {
                var loaded = con.Produkte.Find(prod.Id);
                Assert.Equal(newName, loaded.Name);
            }
        }

        [Fact]
        public void Can_delete_Product()
        {
            var prod = new Produkt() { Farbe = "Gelb", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            using (var con = new ByteBayContext(conString))
            {
                con.Database.EnsureCreated();
                con.Add(prod);
                con.SaveChanges();
            }

            using (var con = new ByteBayContext(conString))
            {
                var loaded = con.Produkte.Find(prod.Id);
                con.Remove(loaded);
                var rows = con.SaveChanges();
                //Assert.Equal(1, rows);
                rows.Should().Be(1);
            }

            using (var con = new ByteBayContext(conString))
            {
                var loaded = con.Produkte.Find(prod.Id);
                //Assert.Null(loaded);
                loaded.Should().BeNull();
            }
        }


        [Fact]
        public void Can_read_Product_AutoFixture_FluentAss()
        {
            var fix = new Fixture();
            fix.Customizations.Add(new PropertyNameOmitter("Id", nameof(Entity.IsDeleted)));
            fix.Behaviors.Add(new OmitOnRecursionBehavior());

            var prod = fix.Create<Produkt>();

            using (var con = new ByteBayContext(conString))
            {
                con.Add(prod);
                con.SaveChanges();
            }

            using (var con = new ByteBayContext(conString))
            {
                var loaded = con.Produkte.Find(prod.Id);

                loaded.Should().BeEquivalentTo(prod, x => x.IgnoringCyclicReferences());
            }
        }

        [Fact]
        public void Delete_Addresse_should_be_set_to_isDeleted()
        {
            var adr = new Adresse() { Zeile1 = "Test123" };
            var best = new Bestellung() { Lieferadresse = adr };
            using (var con = new ByteBayContext(conString))
            {
                con.Add(best);
                con.SaveChanges().Should().Be(2);
            }

            using (var con = new ByteBayContext(conString))
            {
                var loadedAdr = con.Adressen.Find(adr.Id);
                con.Remove(loadedAdr);
                con.SaveChanges();
            }

            using (var con = new ByteBayContext(conString))
            {
                var loadedAdr = con.Adressen.IgnoreQueryFilters().FirstOrDefault(x => x.Id == adr.Id);
                loadedAdr.IsDeleted.Should().BeTrue();
            }
        }

        [Fact]
        public void Delete_Addresse_and_bestellung_should_work()
        {
            var adr = new Adresse() { Zeile1 = "Test123" };
            var best = new Bestellung() { Lieferadresse = adr };
            using (var con = new ByteBayContext(conString))
            {
                con.Add(best);
                con.SaveChanges().Should().Be(2);
            }

            using (var con = new ByteBayContext(conString))
            {
                var loadedAdr = con.Adressen.Find(adr.Id);
                var loadedBest = con.Bestellung.Find(best.Id);
                con.Remove(loadedBest);
                con.Remove(loadedAdr);

                con.SaveChanges().Should().Be(2);
            }
        }


        [Fact]
        public void Delete_Bestllung_should_delete_all_BestellPositionen()
        {
            var prod = new Produkt();
            var best = new Bestellung();
            var pos1 = new BestellPosition() { Bestellung = best, Produkt = prod };
            var pos2 = new BestellPosition() { Bestellung = best, Produkt = prod };

            using (var con = new ByteBayContext(conString))
            {
                //con.Add(best);
                con.Add(pos1);
                con.Add(pos2);
                con.SaveChanges().Should().Be(4);
            }

            using (var con = new ByteBayContext(conString))
            {
                var loadedBest = con.Bestellung.Find(best.Id);
                con.Remove(loadedBest);

                con.SaveChanges().Should().Be(3);
            }

            using (var con = new ByteBayContext(conString))
            {
                con.Bestellung.IgnoreQueryFilters().FirstOrDefault(x => x.Id == best.Id).IsDeleted.Should().BeTrue();
                con.BestellPositionen.IgnoreQueryFilters().FirstOrDefault(x => x.Id == pos1.Id).IsDeleted.Should().BeTrue();
                con.BestellPositionen.IgnoreQueryFilters().FirstOrDefault(x => x.Id == pos2.Id).IsDeleted.Should().BeTrue();
                
                con.Produkte.IgnoreQueryFilters().FirstOrDefault(x => x.Id == prod.Id).IsDeleted.Should().BeFalse();
            }
        }

        [Fact]
        public void Delete_BestellungPosition_should_be_ok()
        {
            var prod = new Produkt();
            var best = new Bestellung();
            var pos1 = new BestellPosition() { Bestellung = best, Produkt = prod };
            var pos2 = new BestellPosition() { Bestellung = best, Produkt = prod };

            using (var con = new ByteBayContext(conString))
            {
                //con.Add(best);
                con.Add(pos1);
                con.Add(pos2);
                con.SaveChanges().Should().Be(4);
            }

            using (var con = new ByteBayContext(conString))
            {
                var loadedBestPos = con.BestellPositionen.Find(pos2.Id);
                con.Remove(loadedBestPos);

                con.SaveChanges().Should().Be(1);
            }

            using (var con = new ByteBayContext(conString))
            {
                con.Bestellung.Find(best.Id).Should().NotBeNull();
                con.BestellPositionen.Find(pos1.Id).Should().NotBeNull();
                con.BestellPositionen.Find(pos2.Id).Should().BeNull();
                con.Produkte.Find(prod.Id).Should().NotBeNull();
            }
        }


        [Fact]
        public void Product_SoftDelete()
        {
            var prod = new Produkt();
            using (var con = new ByteBayContext(conString))
            {
                con.Add(prod);
                con.SaveChanges().Should().Be(1);
            }

            using (var con = new ByteBayContext(conString))
            {
                var loaded = con.Produkte.Find(prod.Id);
                con.Remove(loaded);
                con.SaveChanges().Should().Be(1);
            }

            using (var con = new ByteBayContext(conString))
            {
                var loadedWithFilter = con.Produkte.Where(x => x.Id == prod.Id).FirstOrDefault();
                loadedWithFilter.Should().BeNull();

                var loadedWithoutFilter = con.Produkte.IgnoreQueryFilters().Where(x => x.Id == prod.Id).FirstOrDefault();
                loadedWithoutFilter.Should().NotBeNull();
            }
        }

    }

    internal class PropertyNameOmitter : ISpecimenBuilder
    {
        private readonly IEnumerable<string> names;

        internal PropertyNameOmitter(params string[] names)
        {
            this.names = names;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var propInfo = request as PropertyInfo;
            if (propInfo != null && names.Contains(propInfo.Name))
                return new OmitSpecimen();

            return new NoSpecimen();
        }
    }
}
