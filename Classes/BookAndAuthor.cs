using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookstore.Classes
{
    public class BookAndAuthor
    {
        public BookAndAuthor() { }
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public int? NumberInAuthorGroup { get; set; } 

        public static void InsertBookAndAuthor(int bookId, int authorId, int? numberInAuthorGroup, SqlConnection connection)
        {
            string queryString = @"
    INSERT INTO BooksAuthors (book_id, author_id, number_in_author_group) 
    VALUES (@BookId, @AuthorId, @NumberInAuthorGroup);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@AuthorId", authorId);
                command.Parameters.AddWithValue("@NumberInAuthorGroup", (object)numberInAuthorGroup ?? DBNull.Value);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateBookAndAuthor(int bookId, int oldAuthorId, int newAuthorId, SqlConnection connection)
        {
            string queryString = @"
    UPDATE BooksAuthors
    SET author_id = @NewAuthorId
    WHERE book_id = @BookId AND author_id = @OldAuthorId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@OldAuthorId", oldAuthorId);
                command.Parameters.AddWithValue("@NewAuthorId", newAuthorId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void DeleteBookAndAuthor(int bookId, int authorId, SqlConnection connection)
        {
            string queryString = @"
    DELETE FROM BooksAuthors
    WHERE book_id = @BookId AND author_id = @AuthorId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@AuthorId", authorId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void GetAuthorsOfBook(int bookId, DataGridView dataGridView, SqlConnection connection)
        {
            string query = @"
    SELECT 
        a.author_id AS ID,
        a.author_name + ' ' + a.author_surname AS Імя_та_прізвище, 
        b.number_in_author_group AS Номер_у_групі_авторів
    FROM BooksAuthors b
    INNER JOIN Authors a ON b.author_id = a.author_id
    WHERE b.book_id = @BookId;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable authorsTable = new DataTable();
                    adapter.Fill(authorsTable);

                    dataGridView.DataSource = authorsTable;
                    dataGridView.Columns["ID"].Visible = false; 
                }
            }
        }

        // Get all books for a specific author
        public static void GetBooksOfAuthor(int authorId, DataGridView dataGridView, SqlConnection connection)
        {
            string queryString = @"
            SELECT 
                k.book_name AS Назва_книги
            FROM BooksAuthors b 
            INNER JOIN Books k ON b.book_id = k.book_id
            WHERE b.author_id = @AuthorId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@AuthorId", authorId);

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView.DataSource = dataTable;
                }
            }
        }
    }
}
