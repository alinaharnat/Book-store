using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookstore.Forms
{
    public partial class UsersAdminForm : Form
    {
        public UsersAdminForm()
        {
            InitializeComponent();
        }
        DataBase dataBase = new DataBase();
        private void UsersAdminForm_Load(object sender, EventArgs e)
        {
            EditPanel.Visible = false;
            User.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
        }
        
      
        private bool edit = false;
        private int userId;
        private void addButton_Click(object sender, EventArgs e)
        {
            edit = false;

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;

            user_nameTextBox.Clear();
            user_surnameTextBox.Clear();
            user_middle_nameTextBox.Clear();
            emailTextBox.Clear();
            phoneTextBox.Clear();
            passwordTextBox.Clear();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть користувача для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            edit = true;

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;

            userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["user_id"].Value);
            user_nameTextBox.Text = dataGridView1.SelectedRows[0].Cells["user_name"].Value.ToString();
            user_surnameTextBox.Text = dataGridView1.SelectedRows[0].Cells["user_surname"].Value.ToString();
            user_middle_nameTextBox.Text = dataGridView1.SelectedRows[0].Cells["user_middle_name"].Value.ToString();
            emailTextBox.Text = dataGridView1.SelectedRows[0].Cells["user_email"].Value.ToString();
            phoneTextBox.Text = dataGridView1.SelectedRows[0].Cells["user_phone"].Value.ToString();
            passwordTextBox.Text = dataGridView1.SelectedRows[0].Cells["user_password"].Value.ToString();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int userId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["user_id"].Value);
                User.DeleteUser(userId,connection);
                User.RefreshDataGrid(dataGridView1,connection);
            }
            else
            {
                MessageBox.Show("Виберіть рядок для видалення.");
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            if (edit)
            {
                User.UpdateUser(
                    userId,
                    user_surnameTextBox.Text,
                    user_nameTextBox.Text,
                    user_middle_nameTextBox.Text,
                    emailTextBox.Text,
                    phoneTextBox.Text,
                    passwordTextBox.Text,
                    connection
                );
                User.RefreshDataGrid(dataGridView1, connection);
            }
            else
            {
                User.InsertUser(
                    user_surnameTextBox.Text,
                    user_nameTextBox.Text,
                    user_middle_nameTextBox.Text,
                    emailTextBox.Text,
                    phoneTextBox.Text,
                    passwordTextBox.Text,
                    connection
                );
                User.RefreshDataGrid(dataGridView1,connection);
            }

            EditPanel.Visible = false;
            ButtonsPanel.Visible = true;
        }

        private void canselButton_Click(object sender, EventArgs e)
        {
            EditPanel.Visible = false;
            ButtonsPanel.Visible = true;
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mainLabel_Click(object sender, EventArgs e)
        {
            var form = new AdminMainForm();
            form.ShowDialog();
            this.Close();
        }

        private void booksLabel_Click(object sender, EventArgs e)
        {
            var form = new BookItemsForm();
            form.ShowDialog();
            this.Close();
        }

        private void employeesLabel_Click(object sender, EventArgs e)
        {
            var form = new EmployeesForm();
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
