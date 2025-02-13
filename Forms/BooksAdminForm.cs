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
using Microsoft.SqlServer.Server;
using Bookstore.Classes;
using System.IO;
using System.Data.SqlTypes;

namespace Bookstore.Forms
{
    
    public partial class BooksAdminForm : Form
    {
        DataBase dataBase = new DataBase();
        private bool edit = false;
        public BooksAdminForm()
        {
            InitializeComponent();
        }
      
      
       
        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       

        private void BooksAdminForm_Load(object sender, EventArgs e)
        {
            Book.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
            if (dataGridView1.Columns[1] is DataGridViewImageColumn imageColumn)
            {
                imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
            }
            else
            {

                DataGridViewImageColumn newImageColumn = new DataGridViewImageColumn
                {
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dataGridView1.Columns.RemoveAt(1);
                dataGridView1.Columns.Insert(1, newImageColumn);
            }

            }

        private void addBookButton_Click(object sender, EventArgs e)
        {
            edit = false;
            var form = new AddEditBookForm(edit);
            form.ShowDialog();
        }
        // edit button
        private void addButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть користувача для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            edit = true;
  
            var connection = dataBase.getSqlConnection();
            int selectedBookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            string queryString = @"
            SELECT 
            book_id, book_title, publication_year, book_language, number_of_pages, 
            age, paper_type, binding_type, book_price, book_isbn, book_description, 
            book_rating, stock_quantity, book_photo, publisher_id 
            FROM Books 
            WHERE book_id = @book_id";

            using (connection)
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@book_id", selectedBookId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string book_title = reader.GetString(1);
                            int publication_year = reader.GetInt32(2);
                            string book_language = reader.GetString(3);
                            int number_of_pages = reader.GetInt32(4);
                            int age = reader.GetInt32(5);
                            string paper_type = reader.GetString(6);
                            string binding_type = reader.GetString(7);
                            decimal book_price = reader.GetDecimal(8);
                            string book_isbn = reader.GetString(9);
                            string book_description = reader.GetString(10);
                            decimal book_rating = reader.GetDecimal(11);
                            int stock_quantity = reader.GetInt32(12);
                            byte[] book_photo = reader.IsDBNull(13) ? null : (byte[])reader[13];
                            int publisher_id = reader.GetInt32(14); 

                            var form = new AddEditBookForm(edit,
                                id, book_title, publication_year, book_language, number_of_pages,
                                age, paper_type, binding_type, book_price, book_isbn, book_description,
                                book_rating, stock_quantity, book_photo, publisher_id);  

                            form.ShowDialog();
                        }
                    }
                }
            }
        }

        private byte[] bookPhotoToByteArray(Image image)
        {
            if (image == null)
            {
                return null;
            }
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
        private void deleteButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                //eror
                int bookId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                Book.DeleteBook(bookId,connection);
                Book.RefreshDataGrid(dataGridView1,connection);
            }
            else
            {
                MessageBox.Show("Виберіть рядок для видалення.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0) 
            {
                var connection = dataBase.getSqlConnection();
                authors_DataGridView.DataSource = null;
                subcategories_DataGridView.DataSource = null;

                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                BookAndAuthor.GetAuthorsOfBook(id, authors_DataGridView, connection);
                SubcategoryAndBook.GetSubcategoriesOfBook(id, subcategories_DataGridView, connection);
            }

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string keyword = searchTextBox.Text.Trim(); 
            Book.HighlightRows(keyword, dataGridView1, dataBase.getSqlConnection());
          
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            var form = new BookItemsForm();
            form.ShowDialog();
        }
    }
}
