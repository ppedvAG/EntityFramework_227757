using HalloEfCore.Data;
using HalloEfCore.Model;

namespace HalloEfCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var con = new HalloContext();
            con.Database.EnsureDeleted();
            con.Database.EnsureCreated();

            //MakeDemoData();
            AddBogusDemoDaten();

            dataGridView1.DataSource = con.Customers.ToList();

            //MessageBox.Show("Alles neu");
        }

        private void AddBogusDemoDaten()
        {
            var con = new HalloContext();

            con.Employees.AddRange(DemoData.GetDemoEmployees());
            con.Customers.AddRange(DemoData.GetDemoCustomers());

            con.SaveChanges();
        }

        private void MakeDemoData()
        {
            var con = new HalloContext();

            var p = new Person() { Name = "Nur eine Person" };
            con.Add(p);

            var emp = new Employee() { Name = "Ein Mitarbeiter", Salary = 47234723.4378m };
            con.Add(emp);

            var cust = new Customer() { Name = "Ein Kunde", Address = "zu Hause" };
            con.Add(cust);

            con.SaveChanges();
        }
    }
}