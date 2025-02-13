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
    public partial class ReviewsForm : Form
    {
        public ReviewsForm()
        {
            InitializeComponent();
        }
        DataBase dataBase = new DataBase();
        private bool edit;
        private void ReviewsForm_Load(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            Review.RefreshDataGrid(dataGridView1, connection);
            User.LoadUsersIntoComboBox(user_nameComboBox, connection);
            Book.LoadBooksIntoComboBox(book_titleComboBox, connection);
            EditPanel.Visible = false;
        }

        private void addPublisherButton_Click(object sender, EventArgs e)
        {
            edit = false;
            ButtonsPanel.Visible = false;
            EditPanel.Visible = true;
            book_titleComboBox.SelectedValue = -1;
            user_nameComboBox.SelectedValue = -1;
            review_textTextBox.Clear();
            gradeNumericUpDown.Value = 0;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть відгук для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            edit = true;
            int reviewId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            user_nameComboBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            book_titleComboBox.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            review_textTextBox.Text = dataGridView1.SelectedRows[0].Cells[3].Value?.ToString();
            gradeNumericUpDown.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[4].Value);

            EditPanel.Visible = true;
            ButtonsPanel.Visible = false;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть відгук для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var connection = dataBase.getSqlConnection();

            Review.DeleteReview(selectedId, connection);
            Review.RefreshDataGrid(dataGridView1, connection);
            MessageBox.Show("Автор успішно видалений!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            
            int userId = Convert.ToInt32(user_nameComboBox.SelectedValue);
            int bookId = Convert.ToInt32(book_titleComboBox.SelectedValue);
            int grade = Convert.ToInt32(gradeNumericUpDown.Value);
            DateTime date = DateTime.Now.Date;
            string text = review_textTextBox.Text;

            if (edit)
            {
                if(dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Виберіть відгук для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int reviewId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
              

                Review.UpdateReview(
                    reviewId,
                    userId, 
                    bookId,
                    text,
                    grade,
                    connection
                );
            }
            else
            {
                Review.InsertReview(
                    userId,
                    bookId,
                    text,
                    grade,
                    date,
                    connection
                 );
            }

            Review.RefreshDataGrid(dataGridView1, connection);

            EditPanel.Visible = false;
            ButtonsPanel.Visible = true;

            MessageBox.Show("Відгук успішно збережений!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

