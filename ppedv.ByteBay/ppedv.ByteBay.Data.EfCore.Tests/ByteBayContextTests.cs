using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ppedv.ByteBay.Model;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

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

        [Fact]
        public void Update_Produkt_and_other_update_should_thow_DbUpdateConcurrencyException()
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
                using (var con2 = new ByteBayContext(conString))
                {
                    var otherProd = con2.Produkte.Find(prod.Id);
                    //loaded2.Modified = DateTime.Now;
                    otherProd.Farbe = "blau";
                    con2.SaveChanges();
                }

                loaded.Name = newName;
                Action act = () => con.SaveChanges();
                act.Should().Throw<DbUpdateConcurrencyException>();
            }
        }


        [Fact]
        public void Update_Produkt_and_other_update_should_thow_DbUpdateConcurrencyException_then_DB_wins()
        {
            var prod = new Produkt() { Farbe = "Gelb", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            var newName = $"Bums_{Guid.NewGuid()}";
            using (var con = new ByteBayContext(conString))
            {
                con.Database.EnsureCreated();
                con.Add(prod);
                con.SaveChanges();
            }

            using (var con = new ByteBayContext(conString)) //User 1
            {
                var loaded = con.Produkte.Find(prod.Id);

                using (var con2 = new ByteBayContext(conString)) //User 2 (DB)
                {
                    var otherProd = con2.Produkte.Find(prod.Id);
                    otherProd.Farbe = "blau";
                    con2.SaveChanges();
                }

                loaded.Name = newName;
                Action act = () => con.SaveChanges();
                act.Should().Throw<DbUpdateConcurrencyException>();

                con.Produkte.Find(prod.Id).Farbe.Should().Be("Gelb");
                con.Entry(loaded).Reload(); //DB WINS: lokaler context bekommt hier die Daten auf der DB
                con.Produkte.Find(prod.Id).Farbe.Should().Be("blau");
                con.Produkte.Find(prod.Id).Name.Should().Be(prod.Name);
            }
        }

        [Fact]
        public void Update_Produkt_and_other_update_should_thow_DbUpdateConcurrencyException_then_User_wins()
        {
            var prod = new Produkt() { Farbe = "Gelb", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            var newName = $"Bums_{Guid.NewGuid()}";
            using (var con = new ByteBayContext(conString))
            {
                con.Database.EnsureCreated();
                con.Add(prod);
                con.SaveChanges();
            }

            using (var con = new ByteBayContext(conString)) //User 1
            {
                var loaded = con.Produkte.Find(prod.Id);

                using (var con2 = new ByteBayContext(conString)) //User 2 (DB)
                {
                    var otherProd = con2.Produkte.Find(prod.Id);
                    otherProd.Farbe = "blau";
                    con2.SaveChanges();
                }

                loaded.Name = newName;
                Action act = () => con.SaveChanges();
                act.Should().Throw<DbUpdateConcurrencyException>();


                con.Entry(loaded).OriginalValues.SetValues(con.Entry(loaded).GetDatabaseValues()); //User win, originale werden aus DB geholt
                con.SaveChanges();

                con.Produkte.Find(prod.Id).Farbe.Should().Be("Gelb");
                con.Produkte.Find(prod.Id).Name.Should().Be(newName);

            }
        }

        [Fact]
        public void Two_Contexts_in_one_TransactionScope_Trans_dispose()
        {
            var prod1 = new Produkt() { Farbe = "grün", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            var prod2 = new Produkt() { Farbe = "blau", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            using (var conArrange = new ByteBayContext(conString))
            {
                conArrange.Database.EnsureCreated();
                conArrange.Add(prod1);
                conArrange.Add(prod2);
                conArrange.SaveChanges();
            }

            using (var scope = new TransactionScope())
            {
                using var con1 = new ByteBayContext(conString);
                con1.Produkte.Find(prod1.Id).Farbe = "rot";
                con1.SaveChanges();

                using var con2 = new ByteBayContext(conString);
                con2.Produkte.Find(prod2.Id).Farbe = "rot";
                con2.SaveChanges();

            }

            using (var con = new ByteBayContext(conString))
            {
                con.Produkte.Find(prod1.Id).Farbe.Should().Be("grün");
                con.Produkte.Find(prod2.Id).Farbe.Should().Be("blau");
            }

        }

        [Fact]
        public void Two_Contexts_in_one_TransactionScope_Trans_Complete()
        {
            var prod1 = new Produkt() { Farbe = "grün", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            var prod2 = new Produkt() { Farbe = "blau", Gewicht = 12.60, Name = $"Dings_{Guid.NewGuid()}" };
            using (var conArrange = new ByteBayContext(conString))
            {
                conArrange.Database.EnsureCreated();
                conArrange.Add(prod1);
                conArrange.Add(prod2);
                conArrange.SaveChanges();
            }

            using (var scope = new TransactionScope())
            {
                using var con1 = new ByteBayContext(conString);
                con1.Produkte.Find(prod1.Id).Farbe = "rot";
                con1.SaveChanges();

                using var con2 = new ByteBayContext(conString);
                con2.Produkte.Find(prod2.Id).Farbe = "rot";
                con2.SaveChanges();

                scope.Complete();
            }

            using (var con = new ByteBayContext(conString))
            {
                con.Produkte.Find(prod1.Id).Farbe.Should().Be("rot");
                con.Produkte.Find(prod2.Id).Farbe.Should().Be("rot");
            }



        }


        [Fact]
        public void Fun_with_Group()
        {

            StringBuilder sb = new StringBuilder();
            using (var con = new ByteBayContext(conString))
            {
                //Can_read_Product_AutoFixture_FluentAss();

                var grpQuery = con.Bestellung.Include(x => x.Positionen)
                                             .GroupBy(x => x.BestellDatum.Month)
                                             .Select(x => new { Monat = x.Key, Bests = x.OrderByDescending(x => x.Positionen.Sum(y => y.Preis)).ToList() })
                                             .OrderByDescending(x => x.Monat);

                foreach (var day in grpQuery.ToList())
                {
                    sb.AppendLine(day.Monat.ToString());

                    foreach (var b in day.Bests)
                    {
                        sb.AppendLine($"\t {b.BestellDatum} {b.Positionen.Sum(x => x.Preis)}");
                    }
                }
                //foreach (var day in grpQuery)
                //{
                //    sb.AppendLine(day.Key.ToString());

                //    foreach (var b in day)
                //    {
                //        sb.AppendLine($"\t {b.BestellDatum}");
                //    }
                //}

            }

            throw new Exception(sb.ToString());
        }

        [Fact]
        public void GetAllBestellungenOFProdct()
        {
            using (var con = new ByteBayContext(conString))

            {
                var b1 = con.Bestellung.FirstOrDefault().Positionen.Select(x => x.Produkt.Lieferanten).ToList();

                //var liefers = b1.Positionen.SelectMany(x => x.Produkt.Lieferanten);

            }
        }


        [Fact]
        public void Get_Join()
        {

            List<Tuple<int, string>> zeug = new List<Tuple<int, string>>();
            zeug.Add(new Tuple<int, string>(1, "Toll"));
            zeug.Add(new Tuple<int, string>(2, "Fein"));
            zeug.Add(new Tuple<int, string>(3, "Gut"));
            zeug.Add(new Tuple<int, string>(4, "Hübsch"));

            using (var con = new ByteBayContext(conString))
            {

                var kunden = con.Adressen.Where(x => x.Id >= 1 && x.Id < 5).ToList();

                var beurteilung = kunden.Join(zeug, x => x.Id, x => x.Item1, (x, y) => new { Kunde = x, Text = y.Item2 });

            }
        }

        [Fact]
        public void Get_Join2222()
        {

      
            using (var con = new ByteBayContext(conString))
            {
                var query = from b in con.Set<Adresse>()
                            from p in con.Set<Produkt>()
                            select new { b, p };



                var r = query.ToList();
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
