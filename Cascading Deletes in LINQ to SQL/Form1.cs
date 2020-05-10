using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cascading_Deletes_in_LINQ_to_SQL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Create_Customer()
        {
            try
            {
                Customer c = new Customer();
                c.CustomerID = "AAAAA";
                c.Address = "554 Westwind Avenue";
                c.City = "Wichita";
                c.CompanyName = "Holy Toledo";
                c.ContactName = "Frederick Flintstone";
                c.ContactTitle = "Boss";
                c.Country = "USA";
                c.Fax = "316-335-5933";
                c.Phone = "316-225-4934";
                c.PostalCode = "67214";
                c.Region = "EA";

                Order_Detail od = new Order_Detail();
                od.Discount = .25f;
                od.ProductID = 1;
                od.Quantity = 25;
                od.UnitPrice = 25.00M;

                Order o = new Order();
                o.Order_Details.Add(od);
                o.Freight = 25.50M;
                o.EmployeeID = 1;
                o.CustomerID = "AAAAA";

                c.Orders.Add(o);

                using (NorthwindDataContext dc = new NorthwindDataContext())
                {
                    var table = dc.GetTable<Customer>();
                    table.InsertOnSubmit(c);
                    dc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Create_Customer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (NorthwindDataContext dc = new NorthwindDataContext())
            {

                var q =
                    (from c in dc.GetTable<Customer>()
                     where c.CustomerID == "AAAAA"
                     select c).Single<Customer>();

                dc.GetTable<Customer>().DeleteOnSubmit(q);
                dc.SubmitChanges();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (NorthwindDataContext dc = new NorthwindDataContext())
                {

                    var q =
                        (from c in dc.GetTable<Customer>()
                         where c.CustomerID == "AAAAA"
                         select c).Single<Customer>();

                    foreach (Order ord in q.Orders)
                    {
                        dc.GetTable<Order>().DeleteOnSubmit(ord);

                        foreach (Order_Detail od in ord.Order_Details)
                        {
                            dc.GetTable<Order_Detail>().DeleteOnSubmit(od);
                        }
                    }

                    dc.GetTable<Customer>().DeleteOnSubmit(q);
                    dc.SubmitChanges();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}

