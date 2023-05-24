namespace HalloEfCore.Model
{
    public class Employee : Person
    {
        public decimal Salary { get; set; }

        public virtual ICollection<Department> Departments { get; set; } = new HashSet<Department>();

        public virtual ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();
    }
}
