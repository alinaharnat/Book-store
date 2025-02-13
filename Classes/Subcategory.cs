using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Bookstore.Classes
{
    class Subcategory
    {
        public int SubcategoryId {get; set;}
        public string SubcategoryName { get; set; }
        public int CategoryId { get; set; }

        public Subcategory() { }

        public static void InsertSubcategory(string subcategoryName, SqlConnection connection)
        {
            string queryString = @"
        INSERT INTO Subcategories (subcategory_name)
        VALUES (@SubcategoryName);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@SubcategoryName", subcategoryName);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static void UpdateSubcategory(int subcategoryId, string subcategoryName, SqlConnection connection)
        {
            string queryString = @"
        UPDATE Subcategories
        SET subcategory_name = @SubcategoryName
        WHERE subcategory_id = @SubcategoryId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@SubcategoryId", subcategoryId);
                command.Parameters.AddWithValue("@SubcategoryName", subcategoryName);
            

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static void DeleteSubcategory(int subcategoryId, SqlConnection connection)
        {
            string queryString = "DELETE FROM Subcategories WHERE subcategory_id = @SubcategoryId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@SubcategoryId", subcategoryId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static DataTable GetSubcategories(SqlConnection connection)
        {
            string queryString = @"
            SELECT 
            s.subcategory_id AS 'Id підкатегорії', 
            s.subcategory_name AS 'Назва gідкатегорії'
           
            FROM 
            Subcategories s"; 

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
            DataTable dataTable = GetSubcategories(connection);
            dataGridView.DataSource = dataTable;
        }

        // data for comboBox
        //public static void LoadCategoriesIntoComboBox(ComboBox comboBox, SqlConnection connection)
        //{
        //    string query = "SELECT category_id, category_name FROM Categories";

        //    using (SqlCommand command = new SqlCommand(query, connection))
        //    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        //    {
        //        DataTable categories = new DataTable();
        //        adapter.Fill(categories);

        //        comboBox.DataSource = categories;
        //        comboBox.DisplayMember = "category_name"; 
        //        comboBox.ValueMember = "category_id";    
        //        comboBox.SelectedIndex = -1; 
        //    }
        //}

        public static void LoadSubcategoriesIntoComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string query = "SELECT subcategory_id, subcategory_name FROM Subcategories";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable subcategories = new DataTable();
                adapter.Fill(subcategories);

                comboBox.DataSource = subcategories;
                comboBox.DisplayMember = "subcategory_name";
                comboBox.ValueMember = "subcategory_id";
                comboBox.SelectedIndex = -1;
            }
        }

       


    }
}
