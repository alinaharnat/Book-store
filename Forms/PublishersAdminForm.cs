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
using System.Configuration;
using Bookstore.Classes;

namespace Bookstore.Forms
{
    public partial class PublishersAdminForm : Form
    {
        DataBase dataBase = new DataBase();
        public PublishersAdminForm()
        {
            InitializeComponent();
        }
      
   
        private void PublishersAdminForm_Load(object sender, EventArgs e)
        {
            BookPublisher.RefreshDataGrid(dataGridView1,dataBase.getSqlConnection());
            EditPanel.Visible = false;
            
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //
        private void addBookButton_Click(object sender, EventArgs e)
        {
          
            edit = false;

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;

            publisher_nameTextBox.Clear();
            year_foundedTextBox.Clear();
            publisher_descriptionTextBox.Clear();   
            

        }

        //search
        //private void SearchPublishers(string searchString)
        //{

        //    string queryString = "SELECT * FROM Publishers WHERE publisher_name LIKE '%" + searchString + "%'";

        //    SqlCommand command = new SqlCommand(queryString, dataBase.getSqlConnection());

        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
        //    DataTable dataTable = new DataTable();
        //    dataAdapter.Fill(dataTable);

        //    dataGridView1.DataSource = dataTable;
        //}
        
        private void searchButton_Click(object sender, EventArgs e)
        {
            //var searchString = searchTextBox.Text;
            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    SearchPublishers(searchString); 
            //}
            //else
            //{
            //    RefreshDataGrid(dataGridView1);
                
            //}
        }
        private bool edit;
        private int publisherId;        
        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть видавництво для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            edit = true;

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;

            publisherId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["publisher_id"].Value);
            publisher_nameTextBox.Text  = dataGridView1.SelectedRows[0].Cells["publisher_name"].Value.ToString();
            year_foundedTextBox.Text = dataGridView1.SelectedRows[0].Cells["year_founded"].Value.ToString();
            publisher_descriptionTextBox.Text = dataGridView1.SelectedRows[0].Cells["publisher_description"].Value.ToString();
                  
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var connection  = dataBase.getSqlConnection();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int publisherId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["publisher_id"].Value);

               BookPublisher.DeletePublisher(publisherId,connection);

               BookPublisher.RefreshDataGrid(dataGridView1, connection);
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
                BookPublisher.UpdatePublisher(
                    publisherId, 
                    publisher_nameTextBox.Text, 
                    year_foundedTextBox.Text,
                    publisher_descriptionTextBox.Text, 
                    connection
                );

                BookPublisher.RefreshDataGrid(dataGridView1, connection);

                EditPanel.Visible = false;
                ButtonsPanel.Visible = true;
            }
            else
            {
                BookPublisher.InsertPublisher(
                    publisher_nameTextBox.Text, 
                    year_foundedTextBox.Text, 
                    publisher_descriptionTextBox.Text, 
                    connection
                );

                BookPublisher.RefreshDataGrid(dataGridView1, connection);

                EditPanel.Visible = false;
                ButtonsPanel.Visible = true;
            }

        }

       

        private void canselButton_Click(object sender, EventArgs e)
        {
            EditPanel.Visible = false;
            ButtonsPanel.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
