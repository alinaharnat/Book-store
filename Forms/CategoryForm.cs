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
    public partial class CategoryForm : Form
    {
        public CategoryForm()
        {
            InitializeComponent();
        }
        DataBase dataBase = new DataBase();
        private void CategoryForm_Load(object sender, EventArgs e)
        {
            Category.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
            EditPanel.Visible = false;
        }
       
       
        
        private bool edit = false;
        private int categoryId;
        private void addPublisherButton_Click(object sender, EventArgs e)
        {
            edit = false;
            EditPanel.Visible = true;
            SearchPanel.Visible = false;
            ButtonsPanel.Visible = false;

            category_nameTextBox.Clear();
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
            SearchPanel.Visible = false;
            ButtonsPanel.Visible = false;

            categoryId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["category_id"].Value);
            category_nameTextBox.Text = dataGridView1.SelectedRows[0].Cells["category_name"].Value.ToString();

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int categoryId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["category_id"].Value);

                Category.DeleteCategory(categoryId, connection);

                Category.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
            }
            else
            {
                MessageBox.Show("Виберіть рядок для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            if (edit)
            {
                Category.UpdateCategory(
                    categoryId,
                    category_nameTextBox.Text,
                    connection
                );
                Category.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
            }
            else
            {
                Category.InsertCategory(
                    category_nameTextBox.Text,
                    connection
                );
                Category.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
            }

            EditPanel.Visible = false;
            SearchPanel.Visible = true;
            ButtonsPanel.Visible = true;

        }

      

        private void canselButton_Click(object sender, EventArgs e)
        {
            EditPanel.Visible = false;
            SearchPanel.Visible = true;
            ButtonsPanel.Visible = true;
        }
    }
}
