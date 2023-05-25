namespace ppedv.ByteBay.Model
{
    public class Adresse : Entity
    {
        public string Zeile1 { get; set; } = string.Empty;
        public string? Zeile2 { get; set; }
        public string? Zeile3 { get; set; }
        public string? Zeile4 { get; set; }
        public string Strasse { get; set; } = string.Empty;
        public string Ort { get; set; } = string.Empty;
        public string Plz { get; set; } = string.Empty;
        public string Land { get; set; } = string.Empty;
        public virtual ICollection<Bestellung> BestellungenAlsLieferadresse { get; set; } = new HashSet<Bestellung>();
        public virtual ICollection<Bestellung> BestellungenAlsRechnungsadresse { get; set; } = new HashSet<Bestellung>();
    }
}