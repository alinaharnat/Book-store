using Bookstore.Classes;
using Bookstore.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookstore
{
    public partial class BookItemsForm : Form
    {
        public BookItemsForm()
        {
            InitializeComponent();
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void authorLabel_Click(object sender, EventArgs e)
        {
            var form = new AuthorsAdminForm();
            form.ShowDialog();
        }

        private void BookLabel_Click(object sender, EventArgs e)
        {
            var form = new BooksAdminForm();
            form.ShowDialog();
        }

        private void PublisherLabel_Click(object sender, EventArgs e)
        {
            var form = new PublishersAdminForm();
            form.ShowDialog();
        }

        private void CategoryLabel_Click(object sender, EventArgs e)
        {
            var form = new CategoryForm();
            form.ShowDialog();
        }

        private void SubcategoryLabel_Click(object sender, EventArgs e)
        {
            var form = new SubcategoriesForm();
            form.ShowDialog();
        }

        private void reviewLabel_Click(object sender, EventArgs e)
        {
            var form = new ReviewsForm();
            form.ShowDialog();  

        }

        private void mainLabel_Click(object sender, EventArgs e)
        {
            var form = new AdminMainForm();
            form.ShowDialog();  
            this.Close();
        }

        private void BookItemsForm_Load(object sender, EventArgs e)
        {

        }

        private void employeesLabel_Click(object sender, EventArgs e)
        {
            var form = new EmployeesForm(); 
            form.ShowDialog();
            this.Close();
        }

        private void clientsLable_Click(object sender, EventArgs e)
        {
            var form = new UsersAdminForm();
            form.ShowDialog();
            this.Close();
        }

        private void ordersLabel_Click(object sender, EventArgs e)
        {
            var form = new OrdersForm();
            form.ShowDialog();
            this.Close();
        }

        private void itemsLabel_Click(object sender, EventArgs e)
        {
            var form = new AddressesForm();
            form.ShowDialog();
            this.Close();
        }
    }
}
