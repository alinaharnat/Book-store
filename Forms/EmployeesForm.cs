using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookstore.Forms
{
    public partial class EmployeesForm : Form
    {
        public EmployeesForm()
        {
            InitializeComponent();
        }
        DataBase dataBase = new DataBase();
        private void EmployeesForm_Load(object sender, EventArgs e)
        {
            Employee.RefreshDataGrid(dataGridView1,dataBase.getSqlConnection());
            EditPanel.Visible = false;
        }
       

        // 

        
        private bool edit = false;
        private int employeeId;
        private void addEmployeeButton_Click(object sender, EventArgs e)
        {
            edit = false;

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;

            employee_nameTextBox.Clear();
            employee_surnameTextBox.Clear();
            employee_middle_nameTextBox.Clear();
            emailTextBox.Clear();
            phoneTextBox.Clear();
            passwordTextBox.Clear();
            roleComboBox.SelectedIndex = -1;
            birthdayDateTimePicker.Value = DateTime.Now;
            startDateTimePicker.Value = DateTime.Now;


        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть працівника для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            edit = true;

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;

            employeeId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["employee_id"].Value);
            employee_nameTextBox.Text = dataGridView1.SelectedRows[0].Cells["employee_name"].Value.ToString();
            employee_surnameTextBox.Text = dataGridView1.SelectedRows[0].Cells["employee_surname"].Value.ToString();
            employee_middle_nameTextBox.Text = dataGridView1.SelectedRows[0].Cells["employee_middle_name"].Value.ToString();
            birthdayDateTimePicker.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["employee_birthday_date"].Value);
            roleComboBox.Text = dataGridView1.SelectedRows[0].Cells["role"].Value.ToString();
            startDateTimePicker.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["employee_start_date"].Value);
            emailTextBox.Text = dataGridView1.SelectedRows[0].Cells["employee_email"].Value.ToString();
            phoneTextBox.Text = dataGridView1.SelectedRows[0].Cells["employee_phone"].Value.ToString();
            passwordTextBox.Text = dataGridView1.SelectedRows[0].Cells["employee_password"].Value.ToString();

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
         
                var connection = dataBase.getSqlConnection(); 
               
                    if (edit) 
                    {
                        Employee.UpdateEmployee(
                            employeeId,
                            employee_nameTextBox.Text,
                            employee_surnameTextBox.Text,
                            employee_middle_nameTextBox.Text,
                            birthdayDateTimePicker.Value,
                            roleComboBox.Text,
                            startDateTimePicker.Value,
                            emailTextBox.Text,
                            phoneTextBox.Text,
                            passwordTextBox.Text,
                            connection
                        );
                        Employee.RefreshDataGrid(dataGridView1, connection);
                    }
                    else
                    {
                        Employee.InsertEmployee(
                            employee_nameTextBox.Text,
                            employee_surnameTextBox.Text,
                            employee_middle_nameTextBox.Text,
                            birthdayDateTimePicker.Value,
                            roleComboBox.Text,
                            startDateTimePicker.Value,
                            emailTextBox.Text,
                            phoneTextBox.Text,
                            passwordTextBox.Text,
                            connection
                        );
                        Employee.RefreshDataGrid(dataGridView1, connection);
                    } 

                    EditPanel.Visible = false;
                    ButtonsPanel.Visible = true;

            }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int employeeId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["employee_id"].Value);

                Employee.DeleteEmployee(employeeId, connection);

                Employee.RefreshDataGrid(dataGridView1, connection);
            }
            else
            {
                MessageBox.Show("Виберіть рядок для видалення.");
            }
        }

        private void canselButton_Click(object sender, EventArgs e)
        {
            EditPanel.Visible = false;
            ButtonsPanel.Visible = true;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {

        }

        private void mainLabel_Click(object sender, EventArgs e)
        {
            var form = new AdminMainForm();
            form.Show();
            this.Close();
        }

        private void booksLabel_Click(object sender, EventArgs e)
        {
            var form = new BookItemsForm();
            form.Show();
            this.Close();
        }

        private void clientsLable_Click(object sender, EventArgs e)
        {
            var form = new UsersAdminForm();
            form.Show();
            this.Close();
        }

        private void ordersLabel_Click(object sender, EventArgs e)
        {
            var form = new OrdersForm();
            form.Show();
            this.Close();
        }

        private void itemsLabel_Click(object sender, EventArgs e)
        {
            var form = new AddressesForm(); 
            form.Show();
            this.Close();
        }
    }
    }

