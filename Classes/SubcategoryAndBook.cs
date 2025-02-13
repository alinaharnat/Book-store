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
    public class SubcategoryAndBook
    {
        public int SubcategoryId { get; set; }  
        public int BookId { get; set; }        

        public SubcategoryAndBook() { }
        public static void InsertSubcategoryAndBook(int subcategoryId, int bookId, SqlConnection connection)
        {
            string queryString = @"
    INSERT INTO SubcategoriesBooks (subcategory_id, book_id) 
    VALUES (@SubcategoryId, @BookId);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@SubcategoryId", subcategoryId);
                command.Parameters.AddWithValue("@BookId", bookId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }

        }
        public static void UpdateSubcategoryAndBook(int oldSubcategoryId, int oldBookId, int newSubcategoryId, int newBookId, SqlConnection connection)
        {
            string queryString = @"
UPDATE SubcategoriesBooks
SET subcategory_id = @NewSubcategoryId, book_id = @NewBookId
WHERE subcategory_id = @OldSubcategoryId AND book_id = @OldBookId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@OldSubcategoryId", oldSubcategoryId);
                command.Parameters.AddWithValue("@OldBookId", oldBookId);
                command.Parameters.AddWithValue("@NewSubcategoryId", newSubcategoryId);
                command.Parameters.AddWithValue("@NewBookId", newBookId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new Exception("Запит не оновив жодного запису. Перевірте правильність переданих параметрів.");
                }
            }
        }
        public static void DeleteSubcategoryAndBook(int subcategoryId, int bookId, SqlConnection connection)
        {
            string queryString = @"
    DELETE FROM SubcategoriesBooks
    WHERE subcategory_id = @SubcategoryId AND book_id = @BookId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@SubcategoryId", subcategoryId);
                command.Parameters.AddWithValue("@BookId", bookId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new Exception("Не вдалося видалити запис. Запис може не існувати.");
                }
            }
        }
        public static void GetSubcategoriesOfBook(int id, DataGridView dataGridView, SqlConnection connection)
        {
            string queryString = @"
    SELECT 
        s.subcategory_id AS id,
        s.subcategory_name AS Назва_жанру
    FROM 
        SubcategoriesBooks a
    INNER JOIN 
        Subcategories s ON a.subcategory_id = s.subcategory_id
    INNER JOIN 
        Books k ON a.book_id = k.book_id
    WHERE  
        a.book_id = @BookId";

            DataTable dataTable = new DataTable();

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@BookId", id);

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    dataAdapter.Fill(dataTable);
                }
            }

            dataGridView.DataSource = dataTable;
            dataGridView.Columns["id"].Visible = false;
        }
    }
}
