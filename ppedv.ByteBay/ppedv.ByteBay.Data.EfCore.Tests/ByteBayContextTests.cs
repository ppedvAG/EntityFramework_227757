using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
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
