using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Bookstore.Classes
{
    public class Order
    {
        public Order() { }

        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
        //public string DeliveryCity { get; set; }
        public string DeliveryAddress { get; set; }
        public int? EmployeeId { get; set; }

        public static DataTable GetOrders(SqlConnection connection)
        {
            if (connection == null || string.IsNullOrEmpty(connection.ConnectionString))
            {
                throw new InvalidOperationException("ConnectionString is not initialized.");
            }

            string queryString = @"SELECT
            o.order_id AS ID,
            o.user_id,
            e.employee_id,
            o.payment_method AS Метод_оплати,
            (u.user_name + ' ' + u.user_surname) AS Клієнт,
            o.order_date AS Дата_створення,
            o.order_status AS Статус,
            o.total_price AS Загальна_вартість,
            CONCAT(c.city_name, ', ', d.post_office) AS Адреса_доставки,
            (e.employee_name + ' ' + e.employee_surname) AS Працівник
        FROM Orders o
        LEFT JOIN Users u ON o.user_id = u.user_id
        LEFT JOIN Employees e ON o.employee_id = e.employee_id
        LEFT JOIN DeliveryAddresses d ON o.delivery_address_id = d.delivery_address_id
        LEFT JOIN Cities c ON d.city_id = c.city_id";

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

        public static void InsertOrder(
      string paymentMethod,
      int? userId,
      DateTime orderDate,
      string orderStatus,
      int? deliveryAddressId, 
      int? employeeId,
      SqlConnection connection)
        {
            string queryString = @"
        INSERT INTO Orders 
        (payment_method, user_id, order_date, order_status, total_price, delivery_address_id, employee_id)
        VALUES 
        (@PaymentMethod, @UserId, @OrderDate, @OrderStatus, 0, @DeliveryAddressId, @EmployeeId);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                command.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
                command.Parameters.AddWithValue("@OrderDate", orderDate);
                command.Parameters.AddWithValue("@OrderStatus", orderStatus);
                command.Parameters.AddWithValue("@DeliveryAddressId", (object)deliveryAddressId ?? DBNull.Value);
                command.Parameters.AddWithValue("@EmployeeId", (object)employeeId ?? DBNull.Value);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateOrder(
    int orderId,
    string paymentMethod,
    int? userId,
    DateTime orderDate,
    string orderStatus,
    int? deliveryAddressId, 
    int? employeeId,
    SqlConnection connection)
        {
            string queryString = @"
        UPDATE Orders
        SET 
            payment_method = @PaymentMethod,
            user_id = @UserId,
            order_date = @OrderDate,
            order_status = @OrderStatus,
            delivery_address_id = @DeliveryAddressId,
            employee_id = @EmployeeId
        WHERE order_id = @OrderId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                command.Parameters.AddWithValue("@UserId", (object)userId ?? DBNull.Value);
                command.Parameters.AddWithValue("@OrderDate", orderDate);
                command.Parameters.AddWithValue("@OrderStatus", orderStatus);
                command.Parameters.AddWithValue("@DeliveryAddressId", (object)deliveryAddressId ?? DBNull.Value);
                command.Parameters.AddWithValue("@EmployeeId", (object)employeeId ?? DBNull.Value);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void DeleteOrder(int orderId, SqlConnection connection)
        {
            try
            {
                //Ensure the connection is open before executing the query
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                string query = "DELETE FROM Orders WHERE order_id = @orderId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при видаленні замовлення: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        public static void RefreshDataGrid(DataGridView dataGridView, SqlConnection connection)
        {
            DataTable dataTable = GetOrders(connection);
            dataGridView.DataSource = dataTable;
            dataGridView.Columns["user_id"].Visible = false;
            dataGridView.Columns["employee_id"].Visible = false;
        }

        public static DataTable SearchOrders(string keyword, SqlConnection connection)
        {
            string query = @"
    SELECT CAST(o.order_id AS NVARCHAR) AS result, 'ID замовлення' AS source
    FROM Orders o
    WHERE CAST(o.order_id AS NVARCHAR) LIKE '%' + @Keyword + '%'
    UNION
    SELECT CONCAT(u.user_name, ' ', u.user_surname) AS result, 'Імя користувача' AS source
    FROM Orders o
    LEFT JOIN Users u ON o.user_id = u.user_id
    WHERE LOWER(CONCAT(u.user_name, ' ', u.user_surname)) LIKE '%' + LOWER(@Keyword) + '%'
    UNION
    SELECT CONCAT(e.employee_name, ' ', e.employee_surname) AS result, 'Імя працівника' AS source
    FROM Orders o
    LEFT JOIN Employees e ON o.employee_id = e.employee_id
    WHERE LOWER(CONCAT(e.employee_name, ' ', e.employee_surname)) LIKE '%' + LOWER(@Keyword) + '%'
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
            DataTable searchResults = SearchOrders(keyword, connection);

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
        public static void LoadOrdersIntoComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string query = @"
        SELECT 
            order_id
           
        FROM Orders";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable users = new DataTable();
                adapter.Fill(users);

                comboBox.DataSource = users;
                comboBox.DisplayMember = "order_id";
                comboBox.ValueMember = "order_id";
                comboBox.SelectedIndex = -1;
            }
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
        //for user
        public static DataTable GetUserOrders(SqlConnection connection, int userId)
        {
            string queryString = @"
    SELECT 
        o.order_id AS ID,
        o.payment_method AS Метод_оплати,
        (u.user_name + ' ' + u.user_surname) AS Клієнт,
        o.order_date AS 'Дата',
        o.order_status AS Статус,
        o.total_price AS 'Загальна вартість',
        CONCAT(c.city_name, ', ', d.post_office) AS 'Адреса доставки'
    FROM Orders o
    JOIN Users u ON o.user_id = u.user_id
    LEFT JOIN DeliveryAddresses d ON o.delivery_address_id = d.delivery_address_id
    LEFT JOIN Cities c ON d.city_id = c.city_id
    WHERE u.user_id = @UserId;
";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
        public static void RefreshDataGridForUser(DataGridView dataGridView, SqlConnection connection, int userId)
        {
            DataTable dataTable = GetUserOrders(connection, userId);
            dataGridView.DataSource = dataTable;
        }
        public static string GetOrderStatus(int orderId, SqlConnection connection)
        {
            string status = string.Empty;

            string query = "SELECT status FROM Orders WHERE order_id = @OrderID";

            try
            {
                using (connection)
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            status = result.ToString();
                        }
                        else
                        {
                            status = "Замовлення не знайдено.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message, "Помилка отримання статусу", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return status;
        }
    }
}

