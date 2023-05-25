namespace ppedv.ByteBay.Model
{
    public class BestellPosition : Entity
    {
        public int Menge { get; set; }
        public decimal Preis { get; set; }
        public virtual required Bestellung? Bestellung { get; set; }
        public virtual required Produkt? Produkt { get; set; }
    }
}