namespace ppedv.ByteBay.Model
{
    public class Bestellung : Entity
    {
        public DateTime BestellDatum { get; set; } = DateTime.Now;
        public virtual Adresse? Lieferadresse { get; set; }
        public virtual Adresse? Rechnungsadresse { get; set; }

        public virtual ICollection<BestellPosition> Positionen { get; set; } = new HashSet<BestellPosition>();
    }
}