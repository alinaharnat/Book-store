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
    public class Author
    {
        public int  AuthorId { get; set; } 
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string Biography { get; set; }
        public string Country { get; set; }
        public Author() { }
        public string FullName => $"{AuthorName} {AuthorSurname}";

        public static void InsertAuthor(
        string authorName,
        string authorSurname,
        string biography,
        string country,
        SqlConnection connection)
        {
            string queryString = @"
        INSERT INTO Authors 
        (author_name, author_surname, biography, country)
        VALUES 
        (@AuthorName, @AuthorSurname, @Biography, @Country);";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@AuthorName", authorName);
                command.Parameters.AddWithValue("@AuthorSurname", authorSurname);
                command.Parameters.AddWithValue("@Biography", biography);
                command.Parameters.AddWithValue("@Country", country);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static void UpdateAuthor(
        int authorId,
        string authorName,
        string authorSurname,
        string biography,
        string country,
        SqlConnection connection)
        {
            string queryString = @"
        UPDATE Authors
        SET 
            author_name = @AuthorName,
            author_surname = @AuthorSurname,
            biography = @Biography,
            country = @Country
        WHERE author_id = @AuthorId;";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@AuthorId", authorId);
                command.Parameters.AddWithValue("@AuthorName", authorName);
                command.Parameters.AddWithValue("@AuthorSurname", authorSurname);
                command.Parameters.AddWithValue("@Biography", biography);
                command.Parameters.AddWithValue("@Country", country);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }
        public static void DeleteAuthor(int authorId, SqlConnection connection)
        {
            string queryString = "DELETE FROM Authors WHERE author_id = @AuthorId";

            using (SqlCommand command = new SqlCommand(queryString, connection))
            {
                command.Parameters.AddWithValue("@AuthorId", authorId);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                command.ExecuteNonQuery();
            }
        }

        public static DataTable GetAuthors(SqlConnection connection)
        {
            string queryString = "SELECT * FROM Authors";

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
            DataTable dataTable = GetAuthors(connection);
            dataGridView.DataSource = dataTable;
        }
        public static void LoadAuthorsIntoComboBox(ComboBox comboBox, SqlConnection connection)
        {
            string query = @"
        SELECT 
            author_id, 
            (author_name + ' ' + author_surname) AS FullName 
        FROM Authors";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable authors = new DataTable();
                adapter.Fill(authors);

                comboBox.DataSource = authors;
                comboBox.DisplayMember = "FullName"; 
                comboBox.ValueMember = "author_id";
                comboBox.SelectedIndex = -1;
            }
        }
        public static void LoadAuthorsForFiltration(CheckedListBox checkedListAuthors, SqlConnection connection)
        {
            using ( connection)
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT author_id, (author_name + ' ' + author_surname) AS FullName FROM Authors", connection);
                DataTable authors = new DataTable();
                adapter.Fill(authors);

                checkedListAuthors.Items.Clear();
                foreach (DataRow row in authors.Rows)
                {
                    checkedListAuthors.Items.Add(new KeyValuePair<int, string>((int)row["author_id"], row["FullName"].ToString()));
                }

                checkedListAuthors.DisplayMember = "Value";
                checkedListAuthors.ValueMember = "Key";
            }
        }

    }
}
