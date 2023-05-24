using HalloEfCore.Data;
using HalloEfCore.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Diagnostics;

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

        private void button2_Click(object sender, EventArgs e)
        {
            var con = new HalloContext();

            var query = con.Employees;//rderBy(x=>x.Name);


            dataGridView1.DataSource = query.ToList();

            Debug.WriteLine(query.ToQueryString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var con = new HalloContext();

            var query = from emp in con.Employees
                        where emp.Salary > 1000
                        orderby emp.Salary
                        select emp;

            dataGridView1.DataSource = query.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var con = new HalloContext();

            var bestEmp = con.Employees.FirstOrDefault(x => x.Name.StartsWith("A"));

            MessageBox.Show(bestEmp.Name);
        }
    }
}