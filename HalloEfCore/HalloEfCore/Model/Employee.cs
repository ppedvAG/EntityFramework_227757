namespace HalloEfCore.Model
{
    public class Employee : Person
    {
        public decimal Salary { get; set; }

        public ICollection<Department> Departments { get; set; } = new HashSet<Department>();

        public ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
    }
}
