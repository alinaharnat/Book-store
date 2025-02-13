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
    public partial class SubcategoriesForm : Form
    {
        public SubcategoriesForm()
        {
            InitializeComponent();
        }
        DataBase dataBase = new DataBase();
        private bool edit = false; 
        private int subcategoryId;
        private void SubcategoriesForm_Load(object sender, EventArgs e)
        {
            EditPanel.Visible = false;  
            var connection = dataBase.getSqlConnection();
            Subcategory.RefreshDataGrid(dataGridView1, connection);
        }

        private void addPublisherButton_Click(object sender, EventArgs e)
        {
            edit = false;
            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;

            subcategory_nameTextBox.Clear();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть підкатегорію для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            edit = true;
            subcategoryId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            subcategory_nameTextBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
           // categoryComboBox.SelectedValue = dataGridView1.SelectedRows[0].Cells["category_id"].Value;

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть підкатегорію для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int selectedSubcategoryId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var connection = dataBase.getSqlConnection();

            Subcategory.DeleteSubcategory(selectedSubcategoryId, connection);

            Subcategory.RefreshDataGrid(dataGridView1, connection);
            MessageBox.Show("Підкатегорія успішно видалена!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            if (edit) 
            {
                Subcategory.UpdateSubcategory(
                    subcategoryId,
                    subcategory_nameTextBox.Text,
                    connection
                );
            }
            else 
            {
                Subcategory.InsertSubcategory(
                    subcategory_nameTextBox.Text,
              
                    connection
                );
            }

            Subcategory.RefreshDataGrid(dataGridView1,connection); 

            EditPanel.Visible = false;
            ButtonsPanel.Visible = true;

            MessageBox.Show("Підкатегорія успішно збережена!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
