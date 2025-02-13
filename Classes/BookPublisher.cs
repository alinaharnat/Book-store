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
    public class BookPublisher
    {
        public BookPublisher() { }
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }
        public string YearFounded { get; set; }
        public string PublisherDescription { get; set; }

        public static DataTable GetPublishers(SqlConnection connection)
        {
            string queryString = "SELECT * FROM Publishers";

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
            DataTable dataTable = GetPublishers(connection);
            dataGridView.DataSource = dataTable;
        }

        public static void InsertPublisher(
         string publisherName,
         string yearFounded,
         string publisherDescription,
         SqlConnection connection)
        {
            string queryString = @"
            INSERT INTO Publishers 
            (publisher_name, year_founded, publisher_description)
            VALUES 
            (@PublisherName, @YearFounded, @PublisherDescription);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@PublisherName", publisherName);
                command.Parameters.AddWithValue("@YearFounded", yearFounded);
                command.Parameters.AddWithValue("@PublisherDescription", publisherDescription);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void UpdatePublisher(
            int publisherId,
            string publisherName,
            string yearFounded,
            string publisherDescription,
            SqlConnection connection)
        {
            string queryString = @"
            UPDATE Publishers
            SET 
                publisher_name = @PublisherName,
                year_founded = @YearFounded,
                publisher_description = @PublisherDescription
            WHERE publisher_id = @PublisherId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@PublisherId", publisherId);
                command.Parameters.AddWithValue("@PublisherName", publisherName);
                command.Parameters.AddWithValue("@YearFounded", yearFounded);
                command.Parameters.AddWithValue("@PublisherDescription", publisherDescription);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static void DeletePublisher(int publisherId, SqlConnection connection)
        {
            string queryString = "DELETE FROM Publishers WHERE publisher_id = @PublisherId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@PublisherId", publisherId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void LoadPublishersToComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string queryString = "SELECT publisher_id, publisher_name FROM Publishers";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                comboBox.DataSource = dataTable;
                comboBox.DisplayMember = "publisher_name"; 
                comboBox.ValueMember = "publisher_id";   
            }
        }

    }
}

