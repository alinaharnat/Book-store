using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Bookstore.Forms
{
    public partial class AddNewItemForm : Form
    {
        private int userId;
        private string info;
        private int bookId;
        DataBase dataBase = new DataBase();

        public Cart UserCart { get; set; }

        public AddNewItemForm()
        {
            InitializeComponent();
        }
        public AddNewItemForm(int user_id, string bookInfo, int id)
        {
            InitializeComponent();
           
            this.userId = user_id;
            info = bookInfo;
            bookId = id;
        }
        private void AddNewItemForm_Load(object sender, EventArgs e)
        {
            infoRichTextBox.Text = info;
            Review.LoadReviews(dataGridView1, dataBase.getSqlConnection(), bookId);
            var book_photo = GetBookPhotoFromDatabase(bookId,dataBase.getSqlConnection());
            if(book_photo != null)
            {
                pictureBox1.Image = Image.FromStream(new MemoryStream(book_photo));
            }
            addPanel.Visible = false;
            var availableNumber = GetStockQuantity(bookId, dataBase.getSqlConnection());
            if (availableNumber == 0)
            {
                showAddPanelButton.Visible = false;
                
            }
            else
            {
                showAddPanelButton.Visible = true;
            }
        }

      

        private void addReviewButton_Click(object sender, EventArgs e)
        {
            Review.InsertReview(userId, bookId, reviewTextBox.Text, gradeNumericUpDown.Value, DateTime.Now, dataBase.getSqlConnection());
            gradeNumericUpDown.Value = 0;
            reviewTextBox.Clear();
            Review.LoadReviews(dataGridView1, dataBase.getSqlConnection(), bookId);
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddItemButton_Click(object sender, EventArgs e)
        {
            var availableNumber = GetStockQuantity(bookId, dataBase.getSqlConnection());
            int number = (int)quantityNumericUpDown.Value;
            if (number  > availableNumber) 
            {
                MessageBox.Show($"Доступна максимальна доступна кількість: {availableNumber}");
            }
            else
            {
                var connection = dataBase.getSqlConnection();
                int quantityToAdd = (int)quantityNumericUpDown.Value;
                UserCart.AddItem(bookId, quantityToAdd, connection);
                addPanel.Visible = false;
                showAddPanelButton.Visible = true;
                quantityNumericUpDown.Value = 1;
                MessageBox.Show($"Книга успішно додана до кошика!");
            }
           

        }
        public static int GetStockQuantity(int bookId, SqlConnection connection)
        {
            string queryString = "SELECT stock_quantity FROM Books WHERE book_id = @BookId";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int stockQuantity))
                {
                    return stockQuantity;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void showAddPanelButton_Click(object sender, EventArgs e)
        {
            addPanel.Visible = true;
            showAddPanelButton.Visible = false ;
        }
        private byte[] GetBookPhotoFromDatabase(int bookId, SqlConnection connection)
        {
            byte[] photoData = null;

            
            string query = "SELECT book_photo FROM Books WHERE book_id = @bookID";

            try
            {
                using (connection )
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@bookID", bookId);

                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            photoData = (byte[])result; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка отримання фото: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return photoData; 
        }
    }
}
