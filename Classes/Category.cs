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
    public class Category
    {
        public Category() { }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public static void InsertCategory(
        string categoryName,
        SqlConnection connection)
        {
            string queryString = @"
        INSERT INTO Categories 
        (category_name)
        VALUES 
        (@CategoryName);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@CategoryName", categoryName);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateCategory(
        int categoryId,
        string categoryName,
        SqlConnection connection)
        {
            string queryString = @"
        UPDATE Categories
        SET 
            category_name = @CategoryName
        WHERE category_id = @CategoryId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@CategoryName", categoryName);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static void DeleteCategory(
        int categoryId,
        SqlConnection connection)
        {
            string queryString = "DELETE FROM Categories WHERE category_id = @CategoryId";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@CategoryId", categoryId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static DataTable GetCategories(SqlConnection connection)
        {
            string queryString = "SELECT * FROM Categories";

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
            DataTable dataTable = GetCategories(connection);
            dataGridView.DataSource = dataTable;
        }
    }
}
