using Moq;
using ppedv.ByteBay.Model;
using ppedv.ByteBay.Model.Contracts;

namespace ppedv.ByteBay.Core.Tests
{
    public class OrderServiceTests
    {
        [Fact]
        public void GetMostExpensiveOrderOfToday_Moq()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(x => x.Query<Bestellung>()).Returns(() =>
            {
                var p = new Produkt();
                var b1 = new Bestellung() { BestellDatum = DateTime.Now, Id = 1 };
                b1.Positionen.Add(new BestellPosition { Bestellung = b1, Produkt = p, Preis = 10m, Menge = 2 });
                b1.Positionen.Add(new BestellPosition { Bestellung = b1, Produkt = p, Preis = 5m, Menge = 3 });
                b1.Positionen.Add(new BestellPosition { Bestellung = b1, Produkt = p, Preis = 8m, Menge = 1 });

                var b2 = new Bestellung() { BestellDatum = DateTime.Now, Id = 2 };
                b2.Positionen.Add(new BestellPosition { Bestellung = b2, Produkt = p, Preis = 10m, Menge = 2 });
                b2.Positionen.Add(new BestellPosition { Bestellung = b2, Produkt = p, Preis = 15m, Menge = 300 });
                b2.Positionen.Add(new BestellPosition { Bestellung = b2, Produkt = p, Preis = 8m, Menge = 1 });

                var b3 = new Bestellung() { BestellDatum = DateTime.Now.AddDays(-1), Id = 3 };
                b3.Positionen.Add(new BestellPosition { Bestellung = b3, Produkt = p, Preis = 10m, Menge = 2 });
                b3.Positionen.Add(new BestellPosition { Bestellung = b3, Produkt = p, Preis = 5m, Menge = 100 });
                b3.Positionen.Add(new BestellPosition { Bestellung = b3, Produkt = p, Preis = 8m, Menge = 1 });

                return new[] { b1, b2, b3 }.AsQueryable();
            });

            var service = new OrderService(mock.Object);

            var result = service.GetMostExpensiveOrderOfToday();

            Assert.Equal(2, result.Id);
        }

        [Fact]
        public void GetMostExpensiveOrderOfToday()
        {
            var service = new OrderService(new TestRepo());

            var result = service.GetMostExpensiveOrderOfToday();

            Assert.Equal(2, result.Id);
        }

        [Fact]
        public void CalcOrderSum_ShouldCalculateCorrectOrderSum()
        {
            // Arrange
            var service = new OrderService(null);
            var bestellung = new Bestellung();
            var p = new Produkt();

            bestellung.Positionen.Add(new BestellPosition { Bestellung = bestellung, Produkt = p, Preis = 10m, Menge = 2 }); ;
            bestellung.Positionen.Add(new BestellPosition { Bestellung = bestellung, Produkt = p, Preis = 5m, Menge = 3 });
            bestellung.Positionen.Add(new BestellPosition { Bestellung = bestellung, Produkt = p, Preis = 8m, Menge = 1 });

            var expectedSum = 10m * 2 + 5m * 3 + 8m * 1;

            // Act
            var result = service.CalcOrderSum(bestellung);

            // Assert
            Assert.Equal(expectedSum, result);
        }
    }

    public class TestRepo : IRepository
    {
        public void Add<T>(T entity) where T : Entity
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity) where T : Entity
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query<T>() where T : Entity
        {
            if (typeof(T).IsAssignableFrom(typeof(Bestellung)))
            {
                var p = new Produkt();
                var b1 = new Bestellung() { BestellDatum = DateTime.Now, Id = 1 };
                b1.Positionen.Add(new BestellPosition { Bestellung = b1, Produkt = p, Preis = 10m, Menge = 2 });
                b1.Positionen.Add(new BestellPosition { Bestellung = b1, Produkt = p, Preis = 5m, Menge = 3 });
                b1.Positionen.Add(new BestellPosition { Bestellung = b1, Produkt = p, Preis = 8m, Menge = 1 });

                var b2 = new Bestellung() { BestellDatum = DateTime.Now, Id = 2 };
                b2.Positionen.Add(new BestellPosition { Bestellung = b2, Produkt = p, Preis = 10m, Menge = 2 });
                b2.Positionen.Add(new BestellPosition { Bestellung = b2, Produkt = p, Preis = 15m, Menge = 300 });
                b2.Positionen.Add(new BestellPosition { Bestellung = b2, Produkt = p, Preis = 8m, Menge = 1 });

                var b3 = new Bestellung() { BestellDatum = DateTime.Now.AddDays(-1), Id = 3 };
                b3.Positionen.Add(new BestellPosition { Bestellung = b3, Produkt = p, Preis = 10m, Menge = 2 });
                b3.Positionen.Add(new BestellPosition { Bestellung = b3, Produkt = p, Preis = 5m, Menge = 100 });
                b3.Positionen.Add(new BestellPosition { Bestellung = b3, Produkt = p, Preis = 8m, Menge = 1 });

                return new[] { b1, b2, b3 }.Cast<T>().AsQueryable();
            }

            throw new NotImplementedException();
        }

        public T GetById<T>(int id) where T : Entity
        {
            throw new NotImplementedException();
        }

        public void SaveAll()
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T entity) where T : Entity
        {
            throw new NotImplementedException();
        }
    }
}