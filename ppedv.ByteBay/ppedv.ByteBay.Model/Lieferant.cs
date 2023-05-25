namespace ppedv.ByteBay.Model
{
    public class Lieferant : Entity
    {
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<Produkt> Produkte { get; set; } = new HashSet<Produkt>();

    }
}