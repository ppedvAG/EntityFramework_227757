namespace ppedv.ByteBay.Model
{
    public class Produkt : Entity
    {
        public string Name { get; set; } = string.Empty;
        public string Farbe { get; set; } = string.Empty;
        public double Gewicht { get; set; }
        public virtual ICollection<BestellPosition> Positionen { get; set; } = new HashSet<BestellPosition>();
        public virtual ICollection<Lieferant> Lieferanten { get; set; } = new HashSet<Lieferant>();

    }
}