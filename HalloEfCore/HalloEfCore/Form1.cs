using HalloEfCore.Data;
using HalloEfCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HalloEfCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        HalloContext con = new HalloContext();

        private void button1_Click(object sender, EventArgs e)
        {
            con.Database.EnsureDeleted();
            con.Database.EnsureCreated();

            //MakeDemoData();
            AddBogusDemoDaten();

            dataGridView1.DataSource = con.Customers.ToList();

            //MessageBox.Show("Alles neu");
        }

        private void AddBogusDemoDaten()
        {


            con.Employees.AddRange(DemoData.GetDemoEmployees(customers: DemoData.GetDemoCustomers()));
            //con.Customers.AddRange(DemoData.GetDemoCustomers());

            con.SaveChanges();
        }

        private void MakeDemoData()
        {

            var p = new Person() { Name = "Nur eine Person" };
            con.Add(p);

            var emp = new Employee() { Name = "Ein Mitarbeiter", Salary = 47234723.4378m };
            con.Add(emp);

            var cust = new Customer() { Name = "Ein Kunde", Address = "zu Hause" };
            con.Add(cust);

            con.SaveChanges();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            var query = con.Employees;//rderBy(x=>x.Name);


            dataGridView1.DataSource = query.Include(x => x.Customers).ToList();

            Debug.WriteLine(query.ToQueryString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = con.Employees.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            var query = from emp in con.Employees
                        where emp.Salary > 1000
                        orderby emp.Salary
                        select emp;

            dataGridView1.DataSource = query.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var bestEmp = con.Employees.FirstOrDefault(x => x.Name.StartsWith("A"));

            con.ChangeTracker.Entries().FirstOrDefault(x => x.Entity == bestEmp).State = EntityState.Detached;


            bestEmp.Salary += 1;
            bestEmp.Salary += 1;
            bestEmp.Salary += 1;
            bestEmp.Salary += 1;
            bestEmp.Salary += 1;
            bestEmp.Salary += 1;
            bestEmp.Salary += 1;
            bestEmp.Salary += 1;
            bestEmp.Salary += 1;
            bestEmp.Salary += 1;

            bestEmp.Id = 0;
            con.Attach(bestEmp);

            con.ChangeTracker.Entries().FirstOrDefault(x => x.Entity == bestEmp).State = EntityState.Added;



            con.SaveChanges();


            MessageBox.Show(bestEmp.Name);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            foreach (var enn in con.ChangeTracker.Entries())
            {
                Debug.WriteLine($"{enn.Entity.GetType()} {enn.State}");
            }

            int affectedRows = con.SaveChanges();
            MessageBox.Show($"{affectedRows} Rows changed");
        }

        private void button6_Click(object sender, EventArgs e)
        {

            var emp = con.Employees.FirstOrDefault();
            var changedEmp = new Employee() { Id = emp.Id, Name = "CHÄNGED", Salary = 34287645.2234m };
            con.ChangeTracker.Entries().FirstOrDefault(x => x.Entity == emp).CurrentValues.SetValues(changedEmp);
            con.SaveChanges();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is IEnumerable<Customer> customers)
            {
                e.Value = string.Join(", ", customers.Select(x => x.Name));
            }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.CurrentRow.DataBoundItem is Employee em)
            {
                con.Entry(em).Collection(x => x.Customers).Load(); //explizit Customers laden
                MessageBox.Show($"{em.Name}\n{string.Join(", ", em.Customers.Select(x => x.Name))}");
            }
        }
    }
}