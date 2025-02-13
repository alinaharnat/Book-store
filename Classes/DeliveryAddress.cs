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
    public class DeliveryAddress
    {
        public int DeliveryAddressId { get; set; }
        public int CityId { get; set; }
        public string PostOffice{ get; set; }
        public static void InsertDeliveryAddress(int cityId, string postOffice, SqlConnection connection)
        {

            string query = @"
    INSERT INTO DeliveryAddresses (city_id, post_office)
    VALUES (@CityId, @PostOffice)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                command.Parameters.Add("@PostOffice", SqlDbType.NVarChar, 255).Value = postOffice;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static void UpdateDeliveryAddress(
     string postOffice,
     int deliveryAddressId,
     SqlConnection connection)
        {
            string query = @"
    UPDATE DeliveryAddresses
    SET 
        post_office = @PostOffice
    WHERE delivery_address_id = @DeliveryAddressId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@PostOffice", SqlDbType.NVarChar, 255).Value = postOffice;
                command.Parameters.Add("@DeliveryAddressId", SqlDbType.Int).Value = deliveryAddressId;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static void DeleteDeliveryAddress(int deliveryAddressId, SqlConnection connection)
        {
            string query = "DELETE FROM DeliveryAddresses WHERE delivery_address_id = @DeliveryAddressId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DeliveryAddressId", deliveryAddressId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static DataTable GetDeliveryAddresses(SqlConnection connection, int id)
        {
            DataTable dataTable = new DataTable();

            string query = @"
        SELECT delivery_address_id, 
        post_office AS 'Відділення пошти' 
        FROM DeliveryAddresses  
        WHERE city_id = @CityId;";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CityId", id);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }


        public static void RefreshDataGrid(DataGridView dataGridView, SqlConnection connection,int id)
        {
            DataTable dataTable = GetDeliveryAddresses(connection,id);
            dataGridView.DataSource = dataTable;
           
            dataGridView.Columns["delivery_address_id"].Visible = false;
        }
        public static void LoadDeliveryAddressesIntoComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string query = "SELECT da.delivery_address_id, da.city_id, da.post_office, c.city_name " +
                           "FROM DeliveryAddresses da " +
                           "JOIN Cities c ON da.city_id = c.city_id"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable addresses = new DataTable();
                adapter.Fill(addresses);

                addresses.Columns.Add("FullAddress", typeof(string), "city_name + ', ' + post_office");

                comboBox.DataSource = addresses;
                comboBox.DisplayMember = "FullAddress"; 
                comboBox.ValueMember = "delivery_address_id"; 
                comboBox.SelectedIndex = -1; 
            }
        }
       
        }
}
