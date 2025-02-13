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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Bookstore.Classes;

namespace Bookstore.Forms
{
    public partial class AddEditBookForm : Form
    {
        private int bookId;
        private bool edit;
        private bool editAuthor;
        private bool editSubcategory;
        public AddEditBookForm()
        {
            InitializeComponent();
            
        }
        public AddEditBookForm(bool edit) : this() 
        { 
            this.edit = edit;
            authorAndSubcategoryPanel.Visible = false;
            BookPublisher.LoadPublishersToComboBox(publisher_idComboBox, dataBase.getSqlConnection());
        }
        DataBase dataBase = new DataBase();
       
        public AddEditBookForm(bool edit, int id, string book_title, int publication_year, string book_language, int number_of_pages, int age, string paper_type, string binding_type, decimal book_price, string book_isbn, string book_description, decimal book_rating, int stock_quantity, byte[] book_photo, int publisher_id)
    : this()
        {
            this.edit = edit;
            this.bookId = id;
            book_titleTextBox.Text = book_title;
            publication_yearTextBox.Text = publication_year.ToString();
            book_languageComboBox.Text = book_language;
            number_of_pagesTextBox.Text = number_of_pages.ToString();
            ageTextBox.Text = age.ToString();
            paper_typeTextBox.Text = paper_type;
            binding_typeTextBox.Text = binding_type;
            book_priceTextBox.Text = book_price.ToString();
            book_isbnTextBox.Text = book_isbn;
            book_descriptionTextBox.Text = book_description;
            ratingTextBox.Text = book_rating.ToString();
            stock_quantityTextBox.Text = stock_quantity.ToString();
            BookPublisher.LoadPublishersToComboBox(publisher_idComboBox, dataBase.getSqlConnection());
            publisher_idComboBox.SelectedValue = publisher_id;


            if (book_photo != null)
            {
                pictureBox.Image = Image.FromStream(new MemoryStream(book_photo));
            }
            else
            {
                pictureBox.Image = null;
            }
            BookAndAuthor.GetAuthorsOfBook(id, authors_DataGridView, dataBase.getSqlConnection());
            SubcategoryAndBook.GetSubcategoriesOfBook(id, subcategories_DataGridView, dataBase.getSqlConnection());

        }
        //DataBase dataBase = new DataBase();
        private void Ok_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            try
            {
                var bookTitle = book_titleTextBox.Text;
                var publisherId = (int?)publisher_idComboBox.SelectedValue;
                var publicationYear = int.Parse(publication_yearTextBox.Text);
                var bookLanguage = book_languageComboBox.Text;
                var numberOfPages = int.Parse(number_of_pagesTextBox.Text);
                var bookStatus = int.Parse(stock_quantityTextBox.Text) > 0 ? "В наявності" : "Немає в наявності";
                var age = int.Parse(ageTextBox.Text);
                var paperType = paper_typeTextBox.Text;
                var bindingType = binding_typeTextBox.Text;
                var bookISBN = book_isbnTextBox.Text;
                var bookDescription = book_descriptionTextBox.Text;
                var bookPrice = decimal.Parse(book_priceTextBox.Text);
                var bookRating = decimal.Parse(ratingTextBox.Text);
                var stockQuantity = int.Parse(stock_quantityTextBox.Text);
                var bookPhoto = bookPhotoToByteArray(pictureBox.Image);

                if (!edit)
                {
                    Book.InsertBook(
                        bookTitle,
                        publisherId,
                        publicationYear,
                        bookLanguage,
                        numberOfPages,
                        bookStatus,
                        age,
                        paperType,
                        bindingType,
                        bookISBN,
                        bookDescription,
                        bookPrice,
                        bookRating,
                        stockQuantity,
                        bookPhoto,
                        connection);
                    bookPanel.Visible = false;
                    authorAndSubcategoryPanel.Visible = true;
                    Ok.Visible = false;
                    MessageBox.Show("Книгу успішно додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Book.UpdateBook(
                        bookId,
                        bookTitle,
                        publisherId,
                        publicationYear,
                        bookLanguage,
                        numberOfPages,
                        bookStatus,
                        age,
                        paperType,
                        bindingType,
                        bookISBN,
                        bookDescription,
                        bookPrice,
                        bookRating,
                        stockQuantity,
                        bookPhoto,
                        connection);
                   
                    MessageBox.Show("Книгу успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }



            }
            catch 
            {
                MessageBox.Show($"Перевірте правильність введеих даних", "Помилка", MessageBoxButtons.OK);
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

        private void AddEditBookForm_Load(object sender, EventArgs e)
        {
           
           var connection = dataBase.getSqlConnection();
           Author.LoadAuthorsIntoComboBox(authorsComboBox, connection);
           Subcategory.LoadSubcategoriesIntoComboBox(subcategoriesComboBox, connection);
       
            aBtnPanel.Visible = false;
            sBtnPanel.Visible = false ;
            aPanel.Visible = false;
            subcategoriesComboBox.Visible = false;  
        }

        private void authors_DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(authors_DataGridView.Rows[e.RowIndex].Cells[0].Value);

                authorsComboBox.SelectedValue = id;
                numberTextBox.Text = authors_DataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void subcategories_DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                var connection = dataBase.getSqlConnection();

                int id = Convert.ToInt32(subcategories_DataGridView.Rows[e.RowIndex].Cells[0].Value);

                subcategoriesComboBox.SelectedValue = id;


            }
        }

        private void addPictureBox_Click(object sender, EventArgs e)
        {
            aBtnPanel.Visible = true;
            aPanel.Visible = true ;
            aPicturesPanel.Visible = false;
            authorsComboBox.SelectedIndex = -1;
            numberTextBox.Clear();
            editAuthor = false;

        }

        private void UpdatePictureBox_Click(object sender, EventArgs e)
        {
            aBtnPanel.Visible = true;
            aPicturesPanel.Visible = false;
            aPanel.Visible=true ;
            editAuthor = true;

        }

        private void deletePictureBox_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            if (edit)
            {
                if (authors_DataGridView.SelectedRows.Count > 0)
                {
                    int authorId = Convert.ToInt32(authors_DataGridView.SelectedRows[0].Cells[0].Value);
                    BookAndAuthor.DeleteBookAndAuthor(bookId, authorId, connection);

                    BookAndAuthor.GetAuthorsOfBook(bookId, authors_DataGridView, connection);


                    MessageBox.Show("Автор успішно видалений.");

                }
                else
                {
                    MessageBox.Show("Виберіть автора для видалення.");
                }
            }
            else
            {
                string queryString = "SELECT MAX(book_id) FROM Books";
                int newId;

                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();

                    object result = command.ExecuteScalar();
                    newId = Convert.ToInt32(result);
                }

                if (authors_DataGridView.SelectedRows.Count > 0)
                {
                    int authorId = Convert.ToInt32(authors_DataGridView.SelectedRows[0].Cells[0].Value);
                    BookAndAuthor.DeleteBookAndAuthor(newId, authorId, connection);

                    BookAndAuthor.GetAuthorsOfBook(newId, authors_DataGridView, connection);


                    MessageBox.Show("Автор успішно видалений.");

                }
                else
                {
                    MessageBox.Show("Виберіть автора для видалення.");
                }
            }
        }


        private void aSaveButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            try
            {
                if (edit)
                {


                    if (editAuthor)
                    {
                        if (authors_DataGridView.SelectedRows.Count > 0)
                        {
                            int oldAuthorId = Convert.ToInt32(authors_DataGridView.SelectedRows[0].Cells[0].Value);
                            int newAuthorId = Convert.ToInt32(authorsComboBox.SelectedValue);

                            BookAndAuthor.UpdateBookAndAuthor(bookId, oldAuthorId, newAuthorId, connection);
                            MessageBox.Show("Автор успішно оновлений!");
                        }
                        else
                        {
                            MessageBox.Show("Виберіть рядок у таблиці.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        if (authorsComboBox.SelectedIndex == -1)
                        {
                            MessageBox.Show("Виберіть автора для додавання.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        int authorId = Convert.ToInt32(authorsComboBox.SelectedValue);
                        int? numberInAuthorGroup = string.IsNullOrWhiteSpace(numberTextBox.Text) ? (int?)null : int.Parse(numberTextBox.Text);

                        BookAndAuthor.InsertBookAndAuthor(bookId, authorId, numberInAuthorGroup, connection);
                        MessageBox.Show("Автор успішно доданий до книги!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    BookAndAuthor.GetAuthorsOfBook(bookId, authors_DataGridView, connection);
                }
                else
                {
                    string queryString = "SELECT MAX(book_id) FROM Books";
                    int newId;

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        object result = command.ExecuteScalar();
                        newId = Convert.ToInt32(result);
                    }
                    if (editAuthor)
                    {
                        if (authors_DataGridView.SelectedRows.Count > 0)
                        {
                            int oldAuthorId = Convert.ToInt32(authors_DataGridView.SelectedRows[0].Cells[0].Value);
                            int newAuthorId = Convert.ToInt32(authorsComboBox.SelectedValue);

                            BookAndAuthor.UpdateBookAndAuthor(newId, oldAuthorId, newAuthorId, connection);
                            MessageBox.Show("Автор успішно оновлений!");
                        }
                        else
                        {
                            MessageBox.Show("Виберіть рядок у таблиці.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        if (authorsComboBox.SelectedIndex == -1)
                        {
                            MessageBox.Show("Виберіть автора для додавання.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        int authorId = Convert.ToInt32(authorsComboBox.SelectedValue);
                        int? numberInAuthorGroup = string.IsNullOrWhiteSpace(numberTextBox.Text) ? (int?)null : int.Parse(numberTextBox.Text);

                        BookAndAuthor.InsertBookAndAuthor(newId, authorId, numberInAuthorGroup, connection);
                        MessageBox.Show("Автор успішно доданий до книги!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    BookAndAuthor.GetAuthorsOfBook(newId, authors_DataGridView, connection);

                }


                aBtnPanel.Visible = false;
                aPanel.Visible = false;
                aPicturesPanel.Visible = true;
            }
            catch 
            {
                MessageBox.Show($"Автор та номер у групі авторів не можуть повторюватись.", "Помилка", MessageBoxButtons.OK);
            }
        }

        private void sAddPictureBox_Click(object sender, EventArgs e)
        {
            editSubcategory = false;
            subcategoriesComboBox.Visible = true;
            subcategoriesComboBox.SelectedValue = -1;
            sBtnPanel.Visible = true;
            sPicturePanel.Visible = false;
        }

        private void sSaveButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            if (subcategoriesComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Будь ласка, виберіть підкатегорію.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int subcategoryId = Convert.ToInt32(subcategoriesComboBox.SelectedValue);

            try
            {
                if (edit) 
                {
                    if (editSubcategory) 
                    {
                        if (subcategories_DataGridView.SelectedRows.Count > 0)
                        {
                            int oldSubcategoryId = Convert.ToInt32(subcategories_DataGridView.SelectedRows[0].Cells[0].Value);

                            SubcategoryAndBook.UpdateSubcategoryAndBook(oldSubcategoryId, bookId, subcategoryId, bookId, connection);
                            MessageBox.Show("Зв’язок успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Виберіть підкатегорію для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else 
                    {
                        SubcategoryAndBook.InsertSubcategoryAndBook(subcategoryId, bookId, connection);
                        MessageBox.Show("Новий зв’язок успішно додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    SubcategoryAndBook.GetSubcategoriesOfBook(bookId, subcategories_DataGridView, connection);
                }
                else 
                {
                    string queryString = "SELECT MAX(book_id) FROM Books";
                    int newId;

                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        object result = command.ExecuteScalar();
                        newId = result != DBNull.Value ? Convert.ToInt32(result) : 0;
                    }

                    if (editSubcategory) 
                    {
                        if (subcategories_DataGridView.SelectedRows.Count > 0)
                        {
                            int oldSubcategoryId = Convert.ToInt32(subcategories_DataGridView.SelectedRows[0].Cells[0].Value);

                            SubcategoryAndBook.UpdateSubcategoryAndBook(oldSubcategoryId, newId, subcategoryId, newId, connection);
                            MessageBox.Show("Зв’язок успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Виберіть підкатегорію для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else 
                    {
                        SubcategoryAndBook.InsertSubcategoryAndBook(subcategoryId, newId, connection);
                        MessageBox.Show("Новий зв’язок успішно додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    SubcategoryAndBook.GetSubcategoriesOfBook(newId, subcategories_DataGridView, connection);
                }
            }
            catch 
            {
                MessageBox.Show($"Жанр книги не може дублюватись.", "Помилка", MessageBoxButtons.OK);
            }
            finally
            {
                sBtnPanel.Visible = false;
                sPicturePanel.Visible = true;
                subcategoriesComboBox.Visible = false;
            }
        }

        private void sDeletePictureBox_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            if (edit)
            {
                if (subcategories_DataGridView.SelectedRows.Count > 0)
                {
                    int subcategoryId = Convert.ToInt32(subcategories_DataGridView.SelectedRows[0].Cells[0].Value);

                    try
                    {
                        SubcategoryAndBook.DeleteSubcategoryAndBook(subcategoryId, bookId, connection);
                        SubcategoryAndBook.GetSubcategoriesOfBook(bookId, subcategories_DataGridView, connection);
                        MessageBox.Show("Жанр успішно видалено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch 
                    {
                        MessageBox.Show($"Помилка під час видалення.", "Помилка", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Виберіть жанр для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                string queryString = "SELECT MAX(book_id) FROM Books";
                int newId;

                try
                {
                    using (SqlCommand command = new SqlCommand(queryString, connection))
                    {
                        if (connection.State != ConnectionState.Open)
                            connection.Open();

                        object result = command.ExecuteScalar();
                        newId = Convert.ToInt32(result);
                    }

                    if (subcategories_DataGridView.SelectedRows.Count > 0)
                    {
                        int subcategoryId = Convert.ToInt32(subcategories_DataGridView.SelectedRows[0].Cells[0].Value);

                        SubcategoryAndBook.DeleteSubcategoryAndBook(subcategoryId, newId, connection);
                        SubcategoryAndBook.GetSubcategoriesOfBook(newId, subcategories_DataGridView, connection);
                        MessageBox.Show("Зв’язок з підкатегорією успішно видалено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Виберіть підкатегорію для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка під час видалення: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void sUpdatePictureBox_Click(object sender, EventArgs e)
        {
            editSubcategory = true;
            subcategoriesComboBox.Visible = true;
            subcategoriesComboBox.SelectedValue = Convert.ToInt32(subcategories_DataGridView.SelectedRows[0].Cells[0].Value);
            sBtnPanel.Visible = true;
            sPicturePanel.Visible = false;
            numberTextBox.Text = subcategories_DataGridView.SelectedRows[0].Cells[1].Value.ToString();

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                openFileDialog.Title = "Виберіть зображення";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }

        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var form = new BooksAdminForm();    
            form.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sBtnPanel.Visible = false;
            sPicturePanel.Visible = true;
            subcategoriesComboBox.Visible = false;  


        }
    }
 }
