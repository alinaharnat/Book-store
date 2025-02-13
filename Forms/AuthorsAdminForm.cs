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
using System.Data.SqlClient;

namespace Bookstore.Forms
{
    public partial class AuthorsAdminForm : Form
    {
        public AuthorsAdminForm()
        {
            InitializeComponent();
        }
        DataBase dataBase = new DataBase();
        private bool edit = false;
        int authorId;
        private void AuthorsAdminForm_Load(object sender, EventArgs e)
        {
            Author.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
            EditPanel.Visible = false;
        }

        private void addPublisherButton_Click(object sender, EventArgs e)
        {
            edit = false;

            author_nameTextBox.Clear();
            author_surnameTextBox.Clear();
            bioTextBox.Clear();
            countryComboBox.SelectedIndex = -1;

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть автора для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            edit = true;
            authorId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["author_id"].Value);

            author_nameTextBox.Text = dataGridView1.SelectedRows[0].Cells["author_name"].Value.ToString();
            author_surnameTextBox.Text = dataGridView1.SelectedRows[0].Cells["author_surname"].Value.ToString();
            bioTextBox.Text = dataGridView1.SelectedRows[0].Cells["biography"].Value.ToString();
            countryComboBox.Text = dataGridView1.SelectedRows[0].Cells["country"].Value.ToString();

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть автора для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int selectedAuthorId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["author_id"].Value);
            var connection = dataBase.getSqlConnection();

            Author.DeleteAuthor(selectedAuthorId, connection);
            Author.RefreshDataGrid(dataGridView1,connection);
            MessageBox.Show("Автор успішно видалений!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            if (edit) 
            {
                Author.UpdateAuthor(
                    authorId,
                    author_nameTextBox.Text,
                    author_surnameTextBox.Text,
                    bioTextBox.Text,
                    countryComboBox.Text,
                    connection
                );
            }
            else 
            {
                Author.InsertAuthor(
                    author_nameTextBox.Text,
                    author_surnameTextBox.Text,
                    bioTextBox.Text,
                    countryComboBox.Text,
                    connection
                );
            }

            Author.RefreshDataGrid(dataGridView1, connection); 

            EditPanel.Visible = false;
            ButtonsPanel.Visible = true;

            MessageBox.Show("Автор успішно збережений!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

