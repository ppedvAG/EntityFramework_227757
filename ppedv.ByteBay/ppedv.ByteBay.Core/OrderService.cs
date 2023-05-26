
using ppedv.ByteBay.Model;
using ppedv.ByteBay.Model.Contracts;

namespace ppedv.ByteBay.Core
{
    public class OrderService
    {

        public decimal CalcOrderSum(Bestellung best)
        {
            return best.Positionen.Sum(x => x.Preis * x.Menge);
        }

        public Bestellung GetMostExpensiveOrderOfToday() => GetMostExpensiveOrderOfToday(DateTime.Now);

        public Bestellung? GetMostExpensiveOrderOfToday(DateTime datetime)
        {
            return repo?.Query<Bestellung>()
                       .Where(x => x.BestellDatum.Date == datetime.Date)
                       .OrderByDescending(x => x.Positionen.Sum(y => y.Preis * y.Menge))
                       .FirstOrDefault();
        }

        private IRepository repo;

        public OrderService(IRepository repo)
        {
            this.repo = repo;
        }
    }
}