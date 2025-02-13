using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookstore.Forms
{
    public partial class AddressesForm : Form
    {
        DataBase dataBase = new DataBase();
        private bool edit;
        private bool editAddress;
        public AddressesForm()
        {
            InitializeComponent();
        }

        private void AddressesForm_Load(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            City.RefreshDataGrid(dataGridView1, connection);
            EditPanel.Visible = false;
            AddressEditPanel.Visible = false;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                var connection = dataBase.getSqlConnection();
                DeliveryAddress.RefreshDataGrid(dataGridView2, connection, orderId);
            }
        }

        private void addCityPicture_Click(object sender, EventArgs e)
        {
            edit = false;
            cityTextBox.Clear();
            EditPanel.Visible = true;
            CityButtonsPanel.Visible = false;

        }

        private void UpdateCityPicture_Click(object sender, EventArgs e)
        {
            edit = true;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                CityButtonsPanel.Visible=false; 
                cityTextBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                EditPanel.Visible=true;
            }
            else
            {
                MessageBox.Show("Оберіть місто для редагування.");
            }
        }

        private void DeleteCityPicture_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {

                int cityId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                City.DeleteCity(cityId, dataBase.getSqlConnection());
                City.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
                MessageBox.Show("Місто успішно видалено.");
            }
            else
            {
                MessageBox.Show("Оберіть місто для видалення.");
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string city = cityTextBox.Text;
                int cityId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                if (edit == true)
                {
                    City.UpdateCity(cityId,city,dataBase.getSqlConnection());
                    MessageBox.Show("Місто успішно оновлено.");
                }
                else
                {
                    City.InsertCity(city, dataBase.getSqlConnection());
                    MessageBox.Show("Місто успішно додано.");
                }
                EditPanel.Visible = false;
                CityButtonsPanel.Visible = true;
                City.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
            }
            catch
            {
                MessageBox.Show("Перевірте правильість введеного тексту.");
            }
        }

        private void canselButton_Click(object sender, EventArgs e)
        {
            EditPanel.Visible = false;
            CityButtonsPanel.Visible = true;
        }

        private void AddAddressPicture_Click(object sender, EventArgs e)
        {
            editAddress = false;
            addressTextBox.Clear();
            AddressEditPanel.Visible = true;
            AddressButtonsPanel.Visible = false;

        }

        private void UpdateAddressPicture_Click(object sender, EventArgs e)
        {
            editAddress = true;
            AddressEditPanel.Visible = true;
            AddressButtonsPanel.Visible = false;
            addressTextBox.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void SaveItemButton_Click(object sender, EventArgs e)
        {
            try
            {
                var address = addressTextBox.Text;
                int cityId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                int addressId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                var connection = dataBase.getSqlConnection();
                if (editAddress)
                {
                    DeliveryAddress.UpdateDeliveryAddress(address,addressId, connection);

                    MessageBox.Show("Адреса успішно оновлена!");

                }
                else
                {
                    DeliveryAddress.InsertDeliveryAddress(cityId,address, connection);
                }
                AddressEditPanel.Visible =false;
                AddressButtonsPanel.Visible = true;
                DeliveryAddress.RefreshDataGrid(dataGridView2,connection, cityId);
            }catch 
            {
                MessageBox.Show("Перевірте правильність введеного тексту.");
            }
        }

        private void DeleteIteAddressPicture_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0) 
            {
                int addressId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                var connection = dataBase.getSqlConnection();
                DeliveryAddress.DeleteDeliveryAddress(addressId,connection);
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                DeliveryAddress.RefreshDataGrid(dataGridView2, connection, orderId);
            }
            else
            {
                MessageBox.Show("Оберіть адресу для видалення.");
            }
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ordersLabel_Click(object sender, EventArgs e)
        {
            var form = new OrdersForm();
            form.ShowDialog();
            this.Close();
        }

        private void clientsLable_Click(object sender, EventArgs e)
        {
            var form = new UsersAdminForm();
            form.ShowDialog();
            this.Close();
        }

        private void employeesLabel_Click(object sender, EventArgs e)
        {
            var form = new EmployeesForm();
            form.ShowDialog();
            this.Close();
        }

        private void booksLabel_Click(object sender, EventArgs e)
        {
            var form = new BookItemsForm();
            form.ShowDialog();
            this.Close();
        }

        private void mainLabel_Click(object sender, EventArgs e)
        {
            var form = new AdminMainForm();
            form.ShowDialog();
            this.Close();
        }
    }
}
