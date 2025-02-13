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
    public class Review
    {
        public int ReviewId { get; set; }
        public int? UserId { get; set; }
        public int? BookId { get; set; }
        public string ReviewText { get; set; }
        public int Grade { get; set; }
        public DateTime ReviewDate { get; set; }

        public Review() { }

        public static void InsertReview(int? userId, int? bookId, string reviewText, decimal grade, DateTime reviewDate, SqlConnection connection)
        {
            string query = @"
                INSERT INTO Reviews (user_id, book_id, review_text, grade, review_date) 
                VALUES (@UserId, @BookId, @ReviewText, @Grade, @ReviewDate);";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
                command.Parameters.AddWithValue("@BookId", (object)bookId ?? DBNull.Value);
                command.Parameters.AddWithValue("@ReviewText", (object)reviewText ?? DBNull.Value);
                command.Parameters.AddWithValue("@Grade", grade);
                command.Parameters.AddWithValue("@ReviewDate", reviewDate);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateReview(int reviewId, int? userId, int? bookId, string reviewText, int grade ,SqlConnection connection)
        {
            string query = @"
                UPDATE Reviews
                SET user_id = @UserId, 
                    book_id = @BookId, 
                    review_text = @ReviewText, 
                    grade = @Grade
                 
                WHERE review_id = @ReviewId;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ReviewId", reviewId);
                command.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
                command.Parameters.AddWithValue("@BookId", (object)bookId ?? DBNull.Value);
                command.Parameters.AddWithValue("@ReviewText", (object)reviewText ?? DBNull.Value);
                command.Parameters.AddWithValue("@Grade", grade);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void DeleteReview(int reviewId, SqlConnection connection)
        {
            string query = "DELETE FROM Reviews WHERE review_id = @ReviewId;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ReviewId", reviewId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static DataTable GetReviews(SqlConnection connection)
        {
            string query = @"
                SELECT 
                    r.review_id AS ID,
                    (u.user_name + ' ' + user_surname) AS Імя_та_прізвище,
                    b.book_title AS Назва_книги, 
                    r.review_text AS Текст_відгука, 
                    r.grade AS Оцінка, 
                    r.review_date AS Дата
                FROM Reviews r
                LEFT JOIN Users u ON r.user_id = u.user_id
                LEFT JOIN Books b ON r.book_id = b.book_id;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable reviewsTable = new DataTable();
                    adapter.Fill(reviewsTable);
                    return reviewsTable;
                }
            }
        }

        public static void RefreshDataGrid(DataGridView dataGridView, SqlConnection connection)
        {
            DataTable dataTable = GetReviews(connection);
            dataGridView.DataSource = dataTable;
        }
        public static DataTable GetReviewsForSpecificBook(SqlConnection connection, int bookID)
        {
            string query = @"
        SELECT 
            r.review_id AS ID,
            (u.user_name + ' ' + u.user_surname) AS Імя_та_прізвище,
            r.review_text AS Текст_відгука, 
            r.grade AS Оцінка, 
            r.review_date AS Дата
        FROM Reviews r
        LEFT JOIN Users u ON r.user_id = u.user_id
        LEFT JOIN Books b ON r.book_id = b.book_id 
        WHERE b.book_id = @bookID;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@bookID", bookID);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable reviewsTable = new DataTable();
                    adapter.Fill(reviewsTable);
                    return reviewsTable;
                }
            }
        }
        public static void LoadReviews(DataGridView dataGridView, SqlConnection connection, int bookID)
        {
            DataTable dataTable = GetReviewsForSpecificBook(connection, bookID);
            dataGridView.DataSource = dataTable;
        }
    }
}
