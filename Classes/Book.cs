using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Bookstore.Classes
{
    public class Book
    {
        public int BookId { get; set; }
        public int PublisherId { get; set; }
        public string BookTitle { get; set; }
        public int PublicationYear { get; set; }
        public string BookLanguage { get; set; }
        public int NumberOfPages { get; set; }
        public string BookStatus { get; set; }
        public int Age { get; set; }
        public string PaperType { get; set; }
        public string BindingType { get; set; }
        public string BookISBN { get; set; }
        public string BookDescription { get; set; }
        public decimal BookPrice { get; set; }
        public decimal BookRating { get; set; }
        public int StockQuantity { get; set; }
        public byte[] BookPhoto { get; set; }

        public static void InsertBook(
    string bookTitle,
    int? publisherId,
    int publicationYear,
    string bookLanguage,
    int numberOfPages,
    string bookStatus,
    int? age,
    string paperType,
    string bindingType,
    string bookISBN,
    string bookDescription,
    decimal bookPrice,
    decimal bookRating,
    int stockQuantity,
    byte[] bookPhoto,
    SqlConnection connection)
        {
            string queryString = @"
            INSERT INTO Books (
                book_title, publisher_id, publication_year, book_language, number_of_pages, 
                book_status, age, paper_type, binding_type, book_isbn, book_description, 
                book_price, book_rating, stock_quantity, book_photo)
            VALUES (
                @BookTitle, @PublisherId, @PublicationYear, @BookLanguage, @NumberOfPages, 
                @BookStatus, @Age, @PaperType, @BindingType, @BookISBN, @BookDescription, 
                @BookPrice, @BookRating, @StockQuantity, @BookPhoto)";

            if (bookPrice > 99999999.99m || bookPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(bookPrice), "Book price is out of range.");

            if (bookRating > 99.99m || bookRating < 0)
                throw new ArgumentOutOfRangeException(nameof(bookRating), "Book rating is out of range.");
            var photoParameter = new SqlParameter("@BookPhoto", SqlDbType.VarBinary)
            {
                Value = bookPhoto ?? (object)DBNull.Value
            };


            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@BookTitle", bookTitle);
                command.Parameters.AddWithValue("@PublisherId", (object)publisherId ?? DBNull.Value);
                command.Parameters.AddWithValue("@PublicationYear", publicationYear);
                command.Parameters.AddWithValue("@BookLanguage", bookLanguage);
                command.Parameters.AddWithValue("@NumberOfPages", numberOfPages);
                command.Parameters.AddWithValue("@BookStatus", (object)bookStatus ?? DBNull.Value);
                command.Parameters.AddWithValue("@Age", (object)age ?? DBNull.Value);
                command.Parameters.AddWithValue("@PaperType", (object)paperType ?? DBNull.Value);
                command.Parameters.AddWithValue("@BindingType", (object)bindingType ?? DBNull.Value);
                command.Parameters.AddWithValue("@BookISBN", (object)bookISBN ?? DBNull.Value);
                command.Parameters.AddWithValue("@BookDescription", (object)bookDescription ?? DBNull.Value);
                command.Parameters.AddWithValue("@BookPrice", bookPrice);
                command.Parameters.AddWithValue("@BookRating", bookRating);
                command.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                command.Parameters.Add(photoParameter);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                command.ExecuteNonQuery();
            }
        }
        private byte[] BookPhotoToByteArray(PictureBox pictureBox)
        {
            if (pictureBox.Image == null)
                return null;

            using (var ms = new System.IO.MemoryStream())
            {
                pictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static void UpdateBook(
    int bookId,
    string bookTitle,
    int? publisherId,
    int publicationYear,
    string bookLanguage,
    int numberOfPages,
    string bookStatus,
    int? age,
    string paperType,
    string bindingType,
    string bookISBN,
    string bookDescription,
    decimal bookPrice,
    decimal bookRating,
    int stockQuantity,
    byte[] bookPhoto,
    SqlConnection connection)
        {
            string queryString = @"
    UPDATE Books
    SET 
        book_title = @BookTitle,
        publisher_id = @PublisherId,
        publication_year = @PublicationYear,
        book_language = @BookLanguage,
        number_of_pages = @NumberOfPages,
        book_status = @BookStatus,
        age = @Age,
        paper_type = @PaperType,
        binding_type = @BindingType,
        book_isbn = @BookISBN,
        book_description = @BookDescription,
        book_price = @BookPrice,
        book_rating = @BookRating,
        stock_quantity = @StockQuantity,
        book_photo = @BookPhoto
    WHERE book_id = @BookId";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.Add("@BookId", SqlDbType.Int).Value = bookId;
                command.Parameters.Add("@BookTitle", SqlDbType.NVarChar, 255).Value = bookTitle;
                command.Parameters.Add("@PublisherId", SqlDbType.Int).Value = (object)publisherId ?? DBNull.Value;
                command.Parameters.Add("@PublicationYear", SqlDbType.Int).Value = publicationYear;
                command.Parameters.Add("@BookLanguage", SqlDbType.NVarChar, 50).Value = bookLanguage;
                command.Parameters.Add("@NumberOfPages", SqlDbType.Int).Value = numberOfPages;
                command.Parameters.Add("@BookStatus", SqlDbType.NVarChar, 50).Value = (object)bookStatus ?? DBNull.Value;
                command.Parameters.Add("@Age", SqlDbType.Int).Value = (object)age ?? DBNull.Value;
                command.Parameters.Add("@PaperType", SqlDbType.NVarChar, 50).Value = (object)paperType ?? DBNull.Value;
                command.Parameters.Add("@BindingType", SqlDbType.NVarChar, 50).Value = (object)bindingType ?? DBNull.Value;
                command.Parameters.Add("@BookISBN", SqlDbType.NVarChar, 50).Value = (object)bookISBN ?? DBNull.Value;
                command.Parameters.Add("@BookDescription", SqlDbType.NVarChar).Value = (object)bookDescription ?? DBNull.Value;
                command.Parameters.Add("@BookPrice", SqlDbType.Decimal).Value = bookPrice;
                command.Parameters.Add("@BookRating", SqlDbType.Decimal).Value = bookRating;
                command.Parameters.Add("@StockQuantity", SqlDbType.Int).Value = stockQuantity;
                command.Parameters.Add("@BookPhoto", SqlDbType.VarBinary).Value = (object)bookPhoto ?? DBNull.Value;

                bool shouldCloseConnection = false;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    shouldCloseConnection = true;
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    if (shouldCloseConnection)
                    {
                        connection.Close();
                    }
                }
            }
        
    }

        public static void DeleteBook(int bookId, SqlConnection connection)
        {
            string queryString = "DELETE FROM Books WHERE book_id = @BookId";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                command.ExecuteNonQuery();
            }
        }
        public static DataTable GetBooks(SqlConnection connection)
        {
            string queryString = @"
    SELECT 
        book_id AS Id,
        book_photo AS Фото,
        book_title AS Назва_книги,
        publication_year AS Рік_видання,
        publisher_name AS Назва_видавництва,
        book_price AS Ціна,
        book_rating AS Рейтинг,
        book_language AS Мова,
        number_of_pages AS Кількість_сторінок,
        book_status AS Статус,
        age AS Вік,
        paper_type AS Тип_паперу,
        binding_type AS Тип_обкладинки,
        book_isbn AS Міжнародний_ідентифікатор,
        book_description AS Опис,
        stock_quantity AS Кількість_доступних_одиниць
    FROM 
        Books b 
    INNER JOIN 
        Publishers p 
    ON 
        b.publisher_id = p.publisher_id";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
        public static void RefreshDataGrid(DataGridView dataGridView, SqlConnection connection)
        {
            DataTable dataTable = GetBooks(connection);
            dataGridView.DataSource = dataTable;
        }

        public static void LoadBooksIntoComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string query = "SELECT book_id, book_title FROM Books";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable books = new DataTable();
                adapter.Fill(books);

                comboBox.DataSource = books;
                comboBox.DisplayMember = "book_title";
                comboBox.ValueMember = "book_id";
                comboBox.SelectedIndex = -1;
            }
        }
        public static DataTable SearchBooks(string keyword, SqlConnection connection)
        {
            string query = @"
            SELECT book_title AS result, 'Назва книги' AS source
            FROM Books
            WHERE LOWER(book_title) LIKE '%' + LOWER(@Keyword) + '%'
            UNION
            SELECT book_isbn AS result, 'ISBN книги' AS source
            FROM Books
            WHERE LOWER(book_isbn) LIKE '%' + LOWER(@Keyword) + '%'
            ";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Keyword", keyword.ToLower());

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable resultTable = new DataTable();
                    adapter.Fill(resultTable);
                    return resultTable;
                }
            }
        }

        public static void HighlightRows(string keyword, DataGridView dataGridView, SqlConnection connection)
        {
            DataTable searchResults = SearchBooks(keyword, connection);

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
            dataGridView.ClearSelection();

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataRow searchResult in searchResults.Rows)
                {
                    string searchValue = searchResult["result"].ToString().ToLower();

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchValue))
                        {
                            row.DefaultCellStyle.BackColor = Color.LemonChiffon;
                            break;
                        }
                    }
                }
            }
        }

        //For users
        public static DataTable GetBooksForUsers(SqlConnection connection)
        {
            string queryString = @"
    SELECT 
        book_id AS Id,
        book_photo AS Фото,
        book_title AS Назва_книги,
        publication_year AS Рік_видання,
        publisher_name AS Назва_видавництва,
        book_price AS Ціна,
        book_rating AS Рейтинг,
        book_language AS Мова,
        number_of_pages AS Кількість_сторінок,
        book_status AS Статус,
        age AS Вік,
        paper_type AS Тип_паперу,
        binding_type AS Тип_обкладинки,
        book_isbn AS Міжнародний_ідентифікатор,
        book_description AS Опис
    FROM 
        Books b 
    INNER JOIN 
        Publishers p 
    ON 
        b.publisher_id = p.publisher_id";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public static void RefreshDataGridForUsers(DataGridView dataGridView, SqlConnection connection)
        {
            DataTable dataTable = GetBooksForUsers(connection);
            dataGridView.DataSource = dataTable;
        }

        public static DataTable GetBooksAsDataTable(
      decimal? priceMin,
      decimal? priceMax,
      int? selectedGenreId,  
      List<int> selectedAuthorIds, SqlConnection connection)  
        {
            DataTable booksTable = new DataTable();

            using (connection)
            {
                connection.Open();

                string query = @"
SELECT DISTINCT 
    b.book_id AS Id,
    b.book_photo AS Фото,
    b.book_title AS Назва_книги,
    b.publication_year AS Рік_видання,
    p.publisher_name AS Назва_видавництва,
    (SELECT STRING_AGG(a.author_name + ' ' + a.author_surname, ', ') 
     FROM Authors a
     JOIN BooksAuthors ba ON a.author_id = ba.author_id
     WHERE ba.book_id = b.book_id) AS Автори,
    (SELECT STRING_AGG(g.subcategory_name, ', ') 
     FROM Subcategories g
     JOIN SubcategoriesBooks bg ON g.subcategory_id = bg.subcategory_id
     WHERE bg.book_id = b.book_id) AS Жанри,
    b.book_price AS Ціна,
    b.book_rating AS Рейтинг,
    b.book_language AS Мова,
    b.number_of_pages AS Кількість_сторінок,
    b.book_status AS Статус,
    b.age AS Вік,
    b.paper_type AS Тип_паперу,
    b.binding_type AS Тип_обкладинки,
    b.book_isbn AS Міжнародний_ідентифікатор,
    b.book_description AS Опис
FROM Books b
LEFT JOIN Publishers p ON b.publisher_id = p.publisher_id
WHERE (@PriceMin IS NULL OR b.book_price >= @PriceMin)
  AND (@PriceMax IS NULL OR b.book_price <= @PriceMax)
  AND (@GenreId IS NULL OR b.book_id IN (
      SELECT bg.book_id
      FROM SubcategoriesBooks bg
      WHERE bg.subcategory_id = @GenreId))
  AND (@AuthorIds IS NULL OR b.book_id IN (
      SELECT ba.book_id
      FROM BooksAuthors ba
      WHERE ba.author_id IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@AuthorIds, ','))))
GROUP BY 
    b.book_id, b.book_photo, b.book_title, b.publication_year,
    p.publisher_name, b.book_price, b.book_rating, b.book_language,
    b.number_of_pages, b.book_status, b.age, b.paper_type, 
    b.binding_type, b.book_isbn, b.book_description;



        ";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PriceMin", priceMin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PriceMax", priceMax ?? (object)DBNull.Value);

                command.Parameters.AddWithValue("@GenreId", selectedGenreId ?? (object)DBNull.Value);

                string authorIds = selectedAuthorIds != null && selectedAuthorIds.Count > 0
                    ? string.Join(",", selectedAuthorIds)
                    : null;
                command.Parameters.AddWithValue("@AuthorIds", authorIds ?? (object)DBNull.Value);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
               adapter.Fill(booksTable);
            }

            return booksTable;
        }

        public static string GetBookInfo(DataGridView dataGridView, int rowIndex)
        {
            StringBuilder text = new StringBuilder();

           
                DataGridViewRow row = dataGridView.Rows[rowIndex];

                string bookInfo = $"Назва книги: {row.Cells["Назва_книги"].Value}\n" +
                                  $"Рік видання: {row.Cells["Рік_видання"].Value}\n" +
                                  $"Видавництво: {row.Cells["Назва_видавництва"].Value}\n" +
                                  $"Автори: {row.Cells["Автори"].Value}\n" +
                                  $"Жанри: {row.Cells["Жанри"].Value}\n" +
                                  $"Ціна: {row.Cells["Ціна"].Value} грн\n" +
                                  $"Рейтинг: {row.Cells["Рейтинг"].Value}\n" +
                                  $"Мова: {row.Cells["Мова"].Value}\n" +
                                  $"Кількість сторінок: {row.Cells["Кількість_сторінок"].Value}\n" +
                                  $"Статус: {row.Cells["Статус"].Value}\n" +
                                  $"Вік: {row.Cells["Вік"].Value}\n" +
                                  $"Тип паперу: {row.Cells["Тип_паперу"].Value}\n" +
                                  $"Тип обкладинки: {row.Cells["Тип_обкладинки"].Value}\n" +
                                  $"Міжнародний ідентифікатор: {row.Cells["Міжнародний_ідентифікатор"].Value}\n" +
                                  $"Опис: {row.Cells["Опис"].Value}\n\n";

                text.Append(bookInfo);
            

            return text.ToString();
        }

        //SORT
        public static DataTable SortBooksPriceRating(
    decimal? priceMin,
    decimal? priceMax,
    int? selectedGenreId,
    List<int> selectedAuthorIds,
    SqlConnection connection)
        {
            DataTable booksTable = new DataTable();

            using (connection)
            {
                connection.Open();

                string query = @"
SELECT DISTINCT 
    b.book_id AS Id,
    b.book_photo AS Фото,
    b.book_title AS Назва_книги,
    b.publication_year AS Рік_видання,
    p.publisher_name AS Назва_видавництва,
    (SELECT STRING_AGG(a.author_name + ' ' + a.author_surname, ', ') 
     FROM Authors a
     JOIN BooksAuthors ba ON a.author_id = ba.author_id
     WHERE ba.book_id = b.book_id) AS Автори,
    (SELECT STRING_AGG(g.subcategory_name, ', ') 
     FROM Subcategories g
     JOIN SubcategoriesBooks bg ON g.subcategory_id = bg.subcategory_id
     WHERE bg.book_id = b.book_id) AS Жанри,
    b.book_price AS Ціна,
    b.book_rating AS Рейтинг,
    b.book_language AS Мова,
    b.number_of_pages AS Кількість_сторінок,
    b.book_status AS Статус,
    b.age AS Вік,
    b.paper_type AS Тип_паперу,
    b.binding_type AS Тип_обкладинки,
    b.book_isbn AS Міжнародний_ідентифікатор,
    b.book_description AS Опис
FROM Books b
LEFT JOIN Publishers p ON b.publisher_id = p.publisher_id
WHERE (@PriceMin IS NULL OR b.book_price >= @PriceMin)
  AND (@PriceMax IS NULL OR b.book_price <= @PriceMax)
  AND (@GenreId IS NULL OR b.book_id IN (
      SELECT bg.book_id
      FROM SubcategoriesBooks bg
      WHERE bg.subcategory_id = @GenreId))
  AND (@AuthorIds IS NULL OR b.book_id IN (
      SELECT ba.book_id
      FROM BooksAuthors ba
      WHERE ba.author_id IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@AuthorIds, ','))))
ORDER BY 
    b.book_price ASC, b.book_rating DESC;"; 

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PriceMin", priceMin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PriceMax", priceMax ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@GenreId", selectedGenreId ?? (object)DBNull.Value);

                string authorIds = selectedAuthorIds != null && selectedAuthorIds.Count > 0
                    ? string.Join(",", selectedAuthorIds)
                    : null;
                command.Parameters.AddWithValue("@AuthorIds", authorIds ?? (object)DBNull.Value);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(booksTable);
            }

            return booksTable;
        }
        public static DataTable SortBooksByPages(
   decimal? priceMin,
   decimal? priceMax,
   int? selectedGenreId,
   List<int> selectedAuthorIds,
   SqlConnection connection)
        {
            DataTable booksTable = new DataTable();

            using (connection)
            {
                connection.Open();

                string query = @"
SELECT DISTINCT 
    b.book_id AS Id,
    b.book_photo AS Фото,
    b.book_title AS Назва_книги,
    b.publication_year AS Рік_видання,
    p.publisher_name AS Назва_видавництва,
    (SELECT STRING_AGG(a.author_name + ' ' + a.author_surname, ', ') 
     FROM Authors a
     JOIN BooksAuthors ba ON a.author_id = ba.author_id
     WHERE ba.book_id = b.book_id) AS Автори,
    (SELECT STRING_AGG(g.subcategory_name, ', ') 
     FROM Subcategories g
     JOIN SubcategoriesBooks bg ON g.subcategory_id = bg.subcategory_id
     WHERE bg.book_id = b.book_id) AS Жанри,
    b.book_price AS Ціна,
    b.book_rating AS Рейтинг,
    b.book_language AS Мова,
    b.number_of_pages AS Кількість_сторінок,
    b.book_status AS Статус,
    b.age AS Вік,
    b.paper_type AS Тип_паперу,
    b.binding_type AS Тип_обкладинки,
    b.book_isbn AS Міжнародний_ідентифікатор,
    b.book_description AS Опис
FROM Books b
LEFT JOIN Publishers p ON b.publisher_id = p.publisher_id
WHERE (@PriceMin IS NULL OR b.book_price >= @PriceMin)
  AND (@PriceMax IS NULL OR b.book_price <= @PriceMax)
  AND (@GenreId IS NULL OR b.book_id IN (
      SELECT bg.book_id
      FROM SubcategoriesBooks bg
      WHERE bg.subcategory_id = @GenreId))
  AND (@AuthorIds IS NULL OR b.book_id IN (
      SELECT ba.book_id
      FROM BooksAuthors ba
      WHERE ba.author_id IN (SELECT CAST(value AS INT) FROM STRING_SPLIT(@AuthorIds, ','))))
ORDER BY 
    b.number_of_pages ASC;"; 

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PriceMin", priceMin ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PriceMax", priceMax ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@GenreId", selectedGenreId ?? (object)DBNull.Value);

                string authorIds = selectedAuthorIds != null && selectedAuthorIds.Count > 0
                    ? string.Join(",", selectedAuthorIds)
                    : null;
                command.Parameters.AddWithValue("@AuthorIds", authorIds ?? (object)DBNull.Value);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(booksTable);
            }

            return booksTable;
        }

    }
}

