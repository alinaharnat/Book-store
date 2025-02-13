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
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthdayDate { get; set; }
        public string Role { get; set; }
        public DateTime StartDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public Employee() { }

        public static DataTable GetEmployees(SqlConnection connection)
        {
            string queryString = "SELECT * FROM Employees";

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
       

        public static void InsertEmployee(
            string surname,
            string name,
            string middleName,
            DateTime birthdayDate,
            string role,
            DateTime startDate,
            string email,
            string phone,
            string password,
            SqlConnection connection)
        {
            string queryString = @"
                INSERT INTO Employees 
                (employee_surname, employee_name, employee_middle_name, employee_birthday_date, [role], employee_start_date, employee_email, employee_phone, employee_password)
                VALUES 
                (@Surname, @Name, @MiddleName, @BirthdayDate, @Role, @StartDate, @Email, @Phone, @Password);";

            try
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Surname", SqlDbType.NVarChar) { Value = surname });
                    command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = name });
                    command.Parameters.Add(new SqlParameter("@MiddleName", SqlDbType.NVarChar) { Value = middleName });
                    command.Parameters.Add(new SqlParameter("@BirthdayDate", SqlDbType.Date) { Value = birthdayDate });
                    command.Parameters.Add(new SqlParameter("@Role", SqlDbType.NVarChar) { Value = role });
                    command.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate });
                    command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email });
                    command.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = phone });
                    command.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar) { Value = password });

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час додавання співробітника: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static void UpdateEmployee(
            int employeeId,
            string surname,
            string name,
            string middleName,
            DateTime birthdayDate,
            string role,
            DateTime startDate,
            string email,
            string phone,
            string password,
            SqlConnection connection)
        {
            string queryString = @"
                UPDATE Employees
                SET 
                    employee_surname = @Surname,
                    employee_name = @Name,
                    employee_middle_name = @MiddleName,
                    employee_birthday_date = @BirthdayDate,
                    [role] = @Role,
                    employee_start_date = @StartDate,
                    employee_email = @Email,
                    employee_phone = @Phone,
                    employee_password = @Password
                WHERE employee_id = @EmployeeId;";

            try
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@MiddleName", middleName);
                    command.Parameters.AddWithValue("@BirthdayDate", birthdayDate);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@StartDate", startDate);
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
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час оновлення даних співробітника: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static void DeleteEmployee(int employeeId, SqlConnection connection)
        {
            string queryString = "DELETE FROM Employees WHERE employee_id = @EmployeeId";

            try
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка під час видалення співробітника: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public static void RefreshDataGrid(DataGridView dataGridView, SqlConnection connection)
        {
            DataTable dataTable = GetEmployees(connection);
            dataGridView.DataSource = dataTable;
        }
        public static void LoadEmployeesIntoComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string query = @"
        SELECT 
            employee_id, 
            (employee_name + ' ' + employee_surname) AS FullName 
        FROM Employees";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable employees = new DataTable();
                adapter.Fill(employees);

                comboBox.DataSource = employees;
                comboBox.DisplayMember = "FullName";
                comboBox.ValueMember = "employee_id";
                comboBox.SelectedIndex = -1;
            }
        }
    }
}
