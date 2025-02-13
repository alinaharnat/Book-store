using Bookstore.Classes;
using Bookstore.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookstore
{
    public partial class AdminMainForm : Form
    {
       DataBase dataBase = new DataBase();  
        public AdminMainForm()
        {
            InitializeComponent();
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void booksLabel_Click(object sender, EventArgs e)
        {
            var form = new BookItemsForm();
            this.Close();
            form.ShowDialog();
           
        }

        private void employeesLabel_Click(object sender, EventArgs e)
        {
            var form = new EmployeesForm();
            this.Close();
            form.ShowDialog();
           
        }

        private void clientsLable_Click(object sender, EventArgs e)
        {
            var form = new UsersAdminForm();
            this.Close();
            form.ShowDialog();
          
        }

        private void ordersLabel_Click(object sender, EventArgs e)
        {
            var form = new OrdersForm();
            this.Close();
            form.ShowDialog();
          
        }

        private void AdminMainForm_Load(object sender, EventArgs e)
        {
            mainPanel.BackColor = Color.FromArgb(195, 141, 225);

        }

        private void itemsLabel_Click(object sender, EventArgs e)
        {
            var form = new AddressesForm();
            this.Close();
            form.ShowDialog();
           
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            
        }
        //statistics
        private static void LoadPublisherStatistics(DataGridView dataGridView, SqlConnection connection)
        {
            try
            {
                string query = @"
         SELECT 
        p.publisher_name AS 'Назва видавництв',
        COALESCE(SUM(i.quantity), 0) AS 'Загальна кількість проданих книг', 
        COALESCE(SUM(i.quantity * b.book_price), 0) AS 'Дохід',  
        FORMAT(COALESCE(AVG(b.book_price), 0), 'N2', 'en-US') AS 'Середня вартість книги' 
    FROM 
        Items i
    RIGHT JOIN 
        Books b ON i.book_id = b.book_id
    RIGHT JOIN 
        Publishers p ON b.publisher_id = p.publisher_id
    GROUP BY 
        p.publisher_name
    ORDER BY 
        'Загальна кількість проданих книг' DESC;
";

                using (connection)
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні статистики: {ex.Message}");
            }
        }

        private void GetPublishersStatisticsButton_Click(object sender, EventArgs e)
        {
            LoadPublisherStatistics(dataGridView1,dataBase.getSqlConnection());
        }
        public static void DisplaySalesAnalytics(DataGridView dataGridView, SqlConnection connection)
        {

            string query = @"
        SELECT  
        CASE 
        WHEN MONTH(o.order_date) = 1 THEN 'Січень'
        WHEN MONTH(o.order_date) = 2 THEN 'Лютий'
        WHEN MONTH(o.order_date) = 3 THEN 'Березень'
        WHEN MONTH(o.order_date) = 4 THEN 'Квітень'
        WHEN MONTH(o.order_date) = 5 THEN 'Травень'
        WHEN MONTH(o.order_date) = 6 THEN 'Червень'
        WHEN MONTH(o.order_date) = 7 THEN 'Липень'
        WHEN MONTH(o.order_date) = 8 THEN 'Серпень'
        WHEN MONTH(o.order_date) = 9 THEN 'Вересень'
        WHEN MONTH(o.order_date) = 10 THEN 'Жовтень'
        WHEN MONTH(o.order_date) = 11 THEN 'Листопад'
        WHEN MONTH(o.order_date) = 12 THEN 'Грудень'
        END AS Місяць,
        SUM(i.quantity) AS 'Кількість проданих книг',
        SUM(i.quantity * b.book_price) AS 'Загальна вартість'
        FROM 
        Orders o
        JOIN 
        Items i ON o.order_id = i.order_id
        JOIN 
        Books b ON i.book_id = b.book_id
        WHERE YEAR(o.order_date) = YEAR(GETDATE())
        GROUP BY 
        MONTH(o.order_date)
        ORDER BY 
        'Місяць' DESC;
    ";

            using (connection)
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                try
                {
                    connection.Open();
                    dataAdapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
            private void GetSalesStatisticsButton_Click(object sender, EventArgs e)
        {
            DisplaySalesAnalytics(dataGridView1, dataBase.getSqlConnection());  
        }
        public static void DisplayRepeatPurchaseAnalytics(DataGridView dataGridView, SqlConnection connection)
        {
            string query = @"
        SELECT 
            (u.user_name + ' ' + u.user_surname) AS 'Клієнт',
        
            SUM(i.quantity * b.book_price) AS 'Загальна вартість',
            COUNT(DISTINCT o.order_id) - 1 AS 'Повторні покупки'
        FROM 
            Orders o
        JOIN 
            Items i ON o.order_id = i.order_id
        JOIN 
            Books b ON i.book_id = b.book_id
        JOIN
        Users u ON u.user_id = o.user_id
        GROUP BY 
        u.user_name, u.user_surname
        HAVING 
            COUNT(DISTINCT o.order_id) > 1
        ORDER BY 
            'Повторні покупки' DESC;
    ";

            using (connection)
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                try
                {
                    connection.Open();
                    dataAdapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void GetRepeatedCustomersButton_Click(object sender, EventArgs e)
        {
            DisplayRepeatPurchaseAnalytics(dataGridView1,dataBase.getSqlConnection());
        }
        public static void DisplayCitySalesAnalytics(DataGridView dataGridView, SqlConnection connection)
        {
            string query = @"
        WITH TotalOrders AS (
            SELECT COUNT(DISTINCT order_id) AS total_order_count
            FROM Orders
        ),
        CityOrderCounts AS (
            SELECT 
                c.city_name,
                COUNT(DISTINCT o.order_id) AS city_order_count
            FROM Orders o
            JOIN DeliveryAddresses da ON o.delivery_address_id = da.delivery_address_id
            JOIN Cities c ON da.city_id = c.city_id
            GROUP BY c.city_name
        )
        SELECT 
            c.city_name AS 'Місто',
            c.city_order_count AS 'Кількість замовлень',
            FORMAT((c.city_order_count * 100.0) / (SELECT SUM(city_order_count) FROM CityOrderCounts), 'N2', 'uk-UA') AS 'Відсоток замовлень'
        FROM CityOrderCounts c
        ORDER BY 'Відсоток замовлень' DESC;
    ";

            using (connection)
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                try
                {
                    connection.Open();
                    dataAdapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }




        private void GetCitiesStatisticsButton_Click(object sender, EventArgs e)
        {
            DisplayCitySalesAnalytics(dataGridView1, dataBase.getSqlConnection());  
        }
    }
}
