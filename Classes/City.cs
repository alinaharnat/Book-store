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
    public class City
    {
        public City() { }

        public int CityId { get; set; }
        public string CityName { get; set; }

        public static DataTable GetCities(SqlConnection connection)
        {
            string queryString = "SELECT city_id AS ID, city_name AS Місто FROM Cities";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
           
                return dataTable;
            }
        }

        public static void InsertCity(string cityName, SqlConnection connection)
        {
            string queryString = "INSERT INTO Cities (city_name) VALUES (@CityName);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@CityName", cityName);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                command.ExecuteNonQuery();
            }
        }

        public static void UpdateCity(int cityId, string cityName, SqlConnection connection)
        {
            string queryString = "UPDATE Cities SET city_name = @CityName WHERE city_id = @CityId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@CityId", cityId);
                command.Parameters.AddWithValue("@CityName", cityName);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                command.ExecuteNonQuery();
            }
        }

        public static void DeleteCity(int cityId, SqlConnection connection)
        {
            string queryString = "DELETE FROM Cities WHERE city_id = @CityId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@CityId", cityId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                command.ExecuteNonQuery();
            }
        }

        public static void RefreshDataGrid(DataGridView dataGridView, SqlConnection connection)
        {
            DataTable dataTable = GetCities(connection);
            dataGridView.DataSource = dataTable;
            //dataGridView.Columns[0].Visible = false;
        }

        public static void LoadCitiesIntoComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string query = "SELECT city_id, city_name FROM Cities";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable cities = new DataTable();
                adapter.Fill(cities);

                comboBox.DataSource = cities;
                comboBox.DisplayMember = "city_name";
                comboBox.ValueMember = "city_id";
                comboBox.SelectedIndex = -1;
            }
        }
    }

}
