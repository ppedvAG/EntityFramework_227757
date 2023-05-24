namespace HalloEfCore.Model
{
    public class Customer : Person
    {
        public string Address { get; set; } = string.Empty;

        public Employee? Employee { get; set; }
    }
}
