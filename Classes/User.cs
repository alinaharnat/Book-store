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
    public class User
    {
        public User() { }
        public int UserId { get; set; }
        public string UserSurname { get; set; }
        public string UserName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserPassword { get; set; }

        public static DataTable GetUsers(SqlConnection connection)
        {
            string queryString = "SELECT * FROM Users";

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

        //CRUD operations
        public static void InsertUser(
        string surname,
        string name,
        string middleName,
        string email,
        string phone,
        string password,
        SqlConnection connection)
        {
            string queryString = @"
        INSERT INTO Users 
        (user_surname, user_name, user_middle_name, user_email, user_phone, user_password)
        VALUES 
        (@Surname, @Name, @MiddleName, @Email, @Phone, @Password);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@Surname", surname);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@MiddleName", middleName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Password", password);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateUser(
        int userId,
        string surname,
        string name,
        string middleName,
        string email,
        string phone,
        string password,
        SqlConnection connection)
        {
            string queryString = @"
        UPDATE Users
        SET 
            user_surname = @Surname,
            user_name = @Name,
            user_middle_name = @MiddleName,
            user_email = @Email,
            user_phone = @Phone,
            user_password = @Password
        WHERE user_id = @UserId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Surname", surname);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@MiddleName", middleName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Password", password);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void DeleteUser(int userId, SqlConnection connection)
        {
            string queryString = "DELETE FROM Users WHERE user_id = @UserId";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void RefreshDataGrid(DataGridView dataGridView, SqlConnection connection)
        {
            DataTable dataTable = GetUsers(connection);
            dataGridView.DataSource = dataTable;
        }

        public static void LoadUsersIntoComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string query = @"
        SELECT 
            user_id, 
            (user_name + ' ' + user_surname) AS FullName 
        FROM Users";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable users = new DataTable();
                adapter.Fill(users);

                comboBox.DataSource = users;
                comboBox.DisplayMember = "FullName";
                comboBox.ValueMember = "user_id";
                comboBox.SelectedIndex = -1;
            }
        }
    }
}