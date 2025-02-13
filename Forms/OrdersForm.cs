using Bookstore.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookstore.Classes
{
    public partial class OrdersForm : Form
    {
        private string selectedStatus;
        private decimal? orderMinPrice;
        private decimal? orderMaxPrice;
        private DateTime? startDate;
        private DateTime? endDate;

        public OrdersForm()
        {
            InitializeComponent();
        }

        DataBase dataBase = new DataBase();
        private bool edit;

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            EditPanel.Visible = false;
            Order.RefreshDataGrid(dataGridView1, connection);
            User.LoadUsersIntoComboBox(user_fullNameComboBox, connection);
            Employee.LoadEmployeesIntoComboBox(employeeComboBox, connection);
            DeliveryAddress.LoadDeliveryAddressesIntoComboBox(addressComboBox, connection);
            Book.LoadBooksIntoComboBox(book_idComboBox, connection);
            ItemEditPanel.Visible = false;
        }



        private void addButton_Click(object sender, EventArgs e)
        {
            edit = false;

            user_fullNameComboBox.SelectedIndex = -1;
            employeeComboBox.SelectedIndex = -1;
            paymentComboBox.SelectedIndex = -1;
            order_statusComboBox.SelectedIndex = -1;
            addressComboBox.SelectedIndex = -1;

            EditPanel.Visible = true;
            SearchPanel.Visible = false;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть замовлення для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            edit = true;
            int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            user_fullNameComboBox.SelectedValue = dataGridView1.SelectedRows[0].Cells["user_id"].Value;
            employeeComboBox.SelectedValue = dataGridView1.SelectedRows[0].Cells["employee_id"].Value;
            paymentComboBox.Text = dataGridView1.SelectedRows[0].Cells["Метод_оплати"].Value.ToString();
            order_statusComboBox.Text = dataGridView1.SelectedRows[0].Cells["Статус"].Value.ToString();
            LoadSelectedAddress(dataGridView1,addressComboBox);
            EditPanel.Visible = true;
            SearchPanel.Visible = false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var connection = dataBase.getSqlConnection();

            int? userId = user_fullNameComboBox.SelectedValue as int?;
            int? employeeId = employeeComboBox.SelectedValue as int?;
            int? deliveryAddressId = addressComboBox.SelectedValue as int?;

            if (deliveryAddressId == null)
            {
                MessageBox.Show("Оберіть адресу доставки!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (edit)
            {
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                Order.UpdateOrder(
                    orderId,
                    paymentComboBox.Text,
                    userId,
                    DateTime.Now,
                    order_statusComboBox.Text,
                    deliveryAddressId.Value,
                    employeeId,
                    connection
                );
            }
            else
            {
                Order.InsertOrder(
                    paymentComboBox.Text,
                    userId,
                    DateTime.Now,
                    order_statusComboBox.Text,
                    deliveryAddressId.Value,
                    employeeId,
                    connection
                );
            }

            Order.RefreshDataGrid(dataGridView1, connection);

            EditPanel.Visible = false;
            SearchPanel.Visible = true;
            OrderButtonsPanel.Visible = true;
            MessageBox.Show("Замовлення успішно збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private void canselButton_Click(object sender, EventArgs e)
        {
            EditPanel.Visible = false;
            SearchPanel.Visible = true;
            OrderButtonsPanel.Visible= true;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            string keyword = searchTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                Order.HighlightRows(keyword, dataGridView1, dataBase.getSqlConnection());
            }
            else
            {
                MessageBox.Show("Введіть ключове слово для пошуку.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть замовлення для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int selectedOrderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var connection = dataBase.getSqlConnection();


            
            if (Order.GetOrderStatus(selectedOrderId, connection) == "Нове" || Order.GetOrderStatus(selectedOrderId, connection) == "Прийняте" || Order.GetOrderStatus(selectedOrderId, connection) == "Зібране")
            {
                var list = Item.GetItemIdsForOrder(selectedOrderId, dataBase.getSqlConnection());
                Item.ProcessItems(list, dataBase.getSqlConnection());
                Order.DeleteOrder(selectedOrderId, dataBase.getSqlConnection());
                MessageBox.Show("Замовлення успішно видалено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                Order.DeleteOrder(selectedOrderId,connection);
                MessageBox.Show("Замовлення успішно видалено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            Order.RefreshDataGrid(dataGridView1, connection);
        }

        private static void LoadSelectedAddress(DataGridView dataGridView1, ComboBox addressComboBox)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var selectedValue = dataGridView1.SelectedRows[0].Cells["Адреса_доставки"].Value.ToString();

                if (!string.IsNullOrEmpty(selectedValue))
                {
                    var addressList = (DataTable)addressComboBox.DataSource;
                    var selectedAddress = addressList.AsEnumerable()
                        .FirstOrDefault(a => $"{a["city_name"]}, {a["post_office"]}" == selectedValue);

                    if (selectedAddress != null)
                    {
                        addressComboBox.SelectedValue = selectedAddress["delivery_address_id"];
                    }
                }
            }
        }
        private static void ApplyFilters(string status, DateTime? startDate, DateTime? endDate, DataGridView dataGridView1, SqlConnection connection)
        {
            string query = @"
            SELECT order_id, order_status, order_date 
            FROM Orders
            WHERE 1=1";

            if (!string.IsNullOrEmpty(status))
            {
                query += " AND order_status = @Status";
            }

            if (startDate.HasValue)
            {
                query += " AND order_date >= @StartDate";
            }

            if (endDate.HasValue)
            {
                query += " AND order_date <= @EndDate";
            }

            using (connection)
            {
                SqlCommand command = new SqlCommand(query, connection);

                if (!string.IsNullOrEmpty(status))
                {
                    command.Parameters.AddWithValue("@Status", status);
                }

                if (startDate.HasValue)
                {
                    command.Parameters.AddWithValue("@StartDate", startDate.Value);
                }

                if (endDate.HasValue)
                {
                    command.Parameters.AddWithValue("@EndDate", endDate.Value);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void applyFilterButton_Click(object sender, EventArgs e)
        {
            var filterForm = new FilterOrdersForm(selectedStatus, orderMinPrice, orderMaxPrice, startDate, endDate);

            if (filterForm.ShowDialog() == DialogResult.OK)
            {
                selectedStatus = filterForm.SelectedStatus;
                orderMinPrice = filterForm.OrderMinPrice;
                orderMaxPrice = filterForm.OrderMaxPrice;
                startDate = filterForm.StartDate;
                endDate = filterForm.EndDate;

                dataGridView1.DataSource = ApplyFilters(dataBase.getSqlConnection(), selectedStatus, orderMinPrice, orderMaxPrice, startDate, endDate);
            }
        }

        private void removeFilterButton_Click(object sender, EventArgs e)
        {
            selectedStatus = null;
            orderMinPrice = null;
            orderMaxPrice = null;
            startDate = null;
            endDate = null;

            Order.RefreshDataGrid(dataGridView1, dataBase.getSqlConnection());
        }
        public static DataTable ApplyFilters(SqlConnection connection, string selectedStatus, decimal? orderMinPrice, decimal? orderMaxPrice, DateTime? startDate, DateTime? endDate)
        {
            string queryString = @"
    SELECT 
        o.order_id AS ID,
        u.user_id,
        e.employee_id,
        o.payment_method AS Метод_оплати,
        (u.user_name + ' ' + u.user_surname) AS Клієнт,
        o.order_date AS Дата_створення,
        o.order_status AS Статус,
        o.total_price AS Загальна_вартість,
        CONCAT(c.city_name, ', ', d.post_office) AS Адреса_доставки,
        (e.employee_name + ' ' + e.employee_surname) AS Працівник
    FROM Orders o
    JOIN Users u ON o.user_id = u.user_id
    LEFT JOIN Employees e ON o.employee_id = e.employee_id
    LEFT JOIN DeliveryAddresses d ON o.delivery_address_id = d.delivery_address_id
    LEFT JOIN Cities c ON d.city_id = c.city_id
    WHERE 1=1"; 

            if (!string.IsNullOrEmpty(selectedStatus))
            {
                queryString += " AND o.order_status = @selectedStatus";
            }

            if (orderMinPrice.HasValue)
            {
                queryString += " AND o.total_price >= @orderMinPrice";
            }

            if (orderMaxPrice.HasValue)
            {
                queryString += " AND o.total_price <= @orderMaxPrice";
            }

            if (startDate.HasValue)
            {
                queryString += " AND o.order_date >= @startDate";
            }

            if (endDate.HasValue)
            {
                queryString += " AND o.order_date <= @endDate";
            }

            using (SqlCommand cmd = new SqlCommand(queryString, connection))
            {
                if (!string.IsNullOrEmpty(selectedStatus))
                {
                    cmd.Parameters.AddWithValue("@selectedStatus", selectedStatus);
                }

                if (orderMinPrice.HasValue)
                {
                    cmd.Parameters.AddWithValue("@orderMinPrice", orderMinPrice.Value);
                }

                if (orderMaxPrice.HasValue)
                {
                    cmd.Parameters.AddWithValue("@orderMaxPrice", orderMaxPrice.Value);
                }

                if (startDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate.Value);
                }

                if (endDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@endDate", endDate.Value);
                }

                DataTable dataTable = new DataTable();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(dataTable);
                }

                return dataTable;
            }
        }
      

        public static DataTable GetOrdersInfo(SqlConnection connection, DateTime startDate, DateTime endDate)
        {
            string queryString = @"
    SELECT 
        o.order_id AS ID,
        (u.user_name + ' ' + u.user_surname) AS Клієнт,
        o.order_status AS Статус,
        o.order_date AS Дата_створення,
        o.total_price AS Загальна_вартість
    FROM Orders o
    JOIN Users u ON o.user_id = u.user_id
    WHERE o.order_date BETWEEN @startDate AND @endDate;
    ";

            using (SqlCommand cmd = new SqlCommand(queryString, connection))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                DataTable dataTable = new DataTable();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(dataTable);
                }

                return dataTable;
            }
        }
        public static void GenerateReport(SqlConnection connection, DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                MessageBox.Show("Необхідно вказати початкову та кінцеву дату для формування звіту.", "Помилка", MessageBoxButtons.OK);
                return;
            }

            DataTable ordersData = GetOrdersInfo(connection, startDate.Value, endDate.Value);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.Title = "Зберегти звіт";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Звіт про замовлення за період");
                    writer.WriteLine($"{startDate.Value.ToString("dd.MM.yyyy")} - {endDate.Value.ToString("dd.MM.yyyy")}");
                    writer.WriteLine(new string('-', 110));
                    writer.WriteLine("ID         | Клієнт                         | Статус                    | Дата            | Загальна вартість            ");

                    foreach (DataRow row in ordersData.Rows)
                    {
                        writer.WriteLine($"{row["ID"],-10} | {row["Клієнт"],-30} | {row["Статус"],-25} | {((DateTime)row["Дата_створення"]).ToString("dd.MM.yyyy"),-15} | {row["Загальна_вартість"],-25} ");
                    }

                    writer.WriteLine(new string('-', 110));

                    decimal totalRevenue = ordersData.AsEnumerable().Sum(r => r.Field<decimal>("Загальна_вартість"));
                    decimal averagePrice = ordersData.AsEnumerable().Average(r => r.Field<decimal>("Загальна_вартість"));
                    int totalOrders = ordersData.Rows.Count;

                    writer.WriteLine($"Загальний прибуток: {totalRevenue} грн");
                    writer.WriteLine($"Середня вартість замовлення: {averagePrice} грн");
                    writer.WriteLine($"Загальна кількість замовлень: {totalOrders}");

                    writer.WriteLine(new string('-', 110));
                    var orderStatuses = ordersData.AsEnumerable()
                                                   .GroupBy(r => r.Field<string>("Статус"))
                                                   .Select(g => new { Status = g.Key, Count = g.Count() });

                    writer.WriteLine("Кількість замовлень по статусах:");
                    foreach (var status in orderStatuses)
                    {
                        writer.WriteLine($"{status.Status}: {status.Count}");
                    }

                    writer.WriteLine(new string('-', 110));
                    writer.WriteLine($"Дата формування звіту: {DateTime.Now.ToString("dd.MM.yyyy")}");
                }

                MessageBox.Show("Звіт успішно збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Файл не був збережений.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void generateReportButton_Click(object sender, EventArgs e)
        {
           

                GenerateReport(dataBase.getSqlConnection(), startDate, endDate);
            }

        private void addOrderPicture_Click(object sender, EventArgs e)
        {
            edit = false;

            user_fullNameComboBox.SelectedIndex = -1;
            employeeComboBox.SelectedIndex = -1;
            paymentComboBox.SelectedIndex = -1;
            order_statusComboBox.SelectedIndex = -1;
            addressComboBox.SelectedIndex = -1;
            priсeNumericUpDown.Value = 0;
            OrderButtonsPanel.Visible = false;
            EditPanel.Visible = true;
            SearchPanel.Visible = false;
        }

        private void UpdateOrderPicture_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть замовлення для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            edit = true;
            int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            user_fullNameComboBox.SelectedValue = dataGridView1.SelectedRows[0].Cells["user_id"].Value;
            employeeComboBox.SelectedValue = dataGridView1.SelectedRows[0].Cells["employee_id"].Value;
            paymentComboBox.Text = dataGridView1.SelectedRows[0].Cells["Метод_оплати"].Value.ToString();
            order_statusComboBox.Text = dataGridView1.SelectedRows[0].Cells["Статус"].Value.ToString();
            LoadSelectedAddress(dataGridView1, addressComboBox);
            EditPanel.Visible = true;
            SearchPanel.Visible = false;
            OrderButtonsPanel.Visible = false;
        }

        private void DeleteOrderPicture_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть замовлення для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int selectedOrderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            var connection = dataBase.getSqlConnection();
            var status = dataGridView1.SelectedRows[0].Cells["Статус"].Value.ToString();


            if (status == "Нове" || status == "Прийняте" || status == "Зібране")
            {
                var list = Item.GetItemIdsForOrder(selectedOrderId, dataBase.getSqlConnection());
                Item.ProcessItems(list, dataBase.getSqlConnection());
                Order.DeleteOrder(selectedOrderId, dataBase.getSqlConnection());
                MessageBox.Show("Замовлення успішно видалено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                Order.DeleteOrder(selectedOrderId, connection);
                MessageBox.Show("Замовлення успішно видалено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            Order.RefreshDataGrid(dataGridView1, connection);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                dataGridView2.DataSource = Item.GetOrderItems(dataBase.getSqlConnection(), orderId, dataGridView2);
            }
        }

        private void AddItemPicture_Click(object sender, EventArgs e)
        {
            edit = false;
            book_idComboBox.SelectedIndex = -1;
            quantityNumericUpDown.Value = 0;
            itemButtonsPanel.Visible = false;
            ItemEditPanel.Visible  = true;
        }

        private void UpdateItemPicture_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть позицію для редагування!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            edit = true;
            int currentItemId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
            itemButtonsPanel.Visible = false;
            // order_idComboBox.SelectedValue = dataGridView1.SelectedRows[0].Cells["Номер замовлення"].Value;
            book_idComboBox.SelectedValue = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["book_id"].Value);
            quantityNumericUpDown.Value = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["Кількість"].Value);

            ItemEditPanel.Visible = true;
        }

        private void DeleteItemPicture_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть позицію для видалення!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int itemId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
            var connection = dataBase.getSqlConnection();
            int selectedOrderId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
            var status = dataGridView1.SelectedRows[0].Cells["Статус"].Value.ToString();

            if (status == "Нове" || status == "Прийняте" || status == "Зібране")
            {
                Item.DeleteItemById(itemId, connection);
                Item.RefreshDataGrid(dataGridView2, connection);
                MessageBox.Show("Позиція успішно видалена!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Неможливо видалити позицію замовлення з таким статусом.");
            }

        }

        private void SaveItemButton_Click(object sender, EventArgs e)
        {
            try
            {
                var connection = dataBase.getSqlConnection();

                int bookId = Convert.ToInt32(book_idComboBox.SelectedValue);
                int quantity = Convert.ToInt32(quantityNumericUpDown.Value);
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                if (edit)
                {
                    int itemId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);
                    Item.UpdateItem(
                        itemId,
                        quantity,
                        connection
                    );
                    MessageBox.Show("Позицію успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    try
                    {
                       bool res = Item.InsertItem(
                       orderId,
                       bookId,
                       quantity,
                       connection
                   );
                       if(res == true)
                        {
                            MessageBox.Show("Позицію успішно додано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {

                        }

                    }
                    catch
                    {
                        MessageBox.Show("Книга з такою назвою вже існує в замовленні.");
                    }
                }

                dataGridView2.DataSource = Item.GetOrderItems(connection, orderId, dataGridView2);
                itemButtonsPanel.Visible = true;
                ItemEditPanel.Visible = false;
                SearchPanel.Visible = true;

            }
            catch
            {
                MessageBox.Show("Перевірте правильність введених даних.");
            }
        }

        private void CanselItemButton_Click(object sender, EventArgs e)
        {
            ItemEditPanel.Visible = false;
            itemButtonsPanel.Visible = true;
        }

        private void booksLabel_Click(object sender, EventArgs e)
        {
            var form = new BookItemsForm();
            form.ShowDialog();
        }

        private void employeesLabel_Click(object sender, EventArgs e)
        {
            var form = new EmployeesForm(); 
            form.ShowDialog();  
        }

        private void clientsLable_Click(object sender, EventArgs e)
        {
            var form = new UsersAdminForm();
            form.ShowDialog();  
        }

        private void mainLabel_Click(object sender, EventArgs e)
        {
            var form = new AdminMainForm();
            form.ShowDialog();  
        }

        private void itemsLabel_Click(object sender, EventArgs e)
        {
            var form = new AddressesForm();
            form.ShowDialog();  
        }
        public static DataTable SortByPrice(
      SqlConnection connection,
      string selectedStatus,
      decimal? orderMinPrice,
      decimal? orderMaxPrice,
      DateTime? startDate,
      DateTime? endDate)
        {
            string queryString = @"
SELECT 
    o.order_id AS ID,
    u.user_id,
    e.employee_id,
    o.payment_method AS Метод_оплати,
    (u.user_name + ' ' + u.user_surname) AS Клієнт,
    o.order_date AS Дата_створення,
    o.order_status AS Статус,
    o.total_price AS Загальна_вартість,
    CONCAT(c.city_name, ', ', d.post_office) AS Адреса_доставки,
    (e.employee_name + ' ' + e.employee_surname) AS Працівник
FROM Orders o
JOIN Users u ON o.user_id = u.user_id
LEFT JOIN Employees e ON o.employee_id = e.employee_id
LEFT JOIN DeliveryAddresses d ON o.delivery_address_id = d.delivery_address_id
LEFT JOIN Cities c ON d.city_id = c.city_id
WHERE 1=1";  
            if (!string.IsNullOrEmpty(selectedStatus))
            {
                queryString += " AND o.order_status = @selectedStatus";
            }

            if (orderMinPrice.HasValue)
            {
                queryString += " AND o.total_price >= @orderMinPrice";
            }

            if (orderMaxPrice.HasValue)
            {
                queryString += " AND o.total_price <= @orderMaxPrice";
            }

            if (startDate.HasValue)
            {
                queryString += " AND o.order_date >= @startDate";
            }

            if (endDate.HasValue)
            {
                queryString += " AND o.order_date <= @endDate";
            }

            queryString += " ORDER BY o.total_price ASC;";  

            using (SqlCommand cmd = new SqlCommand(queryString, connection))
            {
                if (!string.IsNullOrEmpty(selectedStatus))
                {
                    cmd.Parameters.AddWithValue("@selectedStatus", selectedStatus);
                }

                if (orderMinPrice.HasValue)
                {
                    cmd.Parameters.AddWithValue("@orderMinPrice", orderMinPrice.Value);
                }

                if (orderMaxPrice.HasValue)
                {
                    cmd.Parameters.AddWithValue("@orderMaxPrice", orderMaxPrice.Value);
                }

                if (startDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate.Value);
                }

                if (endDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@endDate", endDate.Value);
                }

                DataTable dataTable = new DataTable();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(dataTable);  
                }

                return dataTable;
            }

        }
        public static DataTable SortByDate(SqlConnection connection, string selectedStatus, decimal? orderMinPrice, decimal? orderMaxPrice, DateTime? startDate, DateTime? endDate)
        {
            string queryString = @"
SELECT 
    o.order_id AS ID,
    u.user_id,
    e.employee_id,
    o.payment_method AS Метод_оплати,
    (u.user_name + ' ' + u.user_surname) AS Клієнт,
    o.order_date AS Дата_створення,
    o.order_status AS Статус,
    o.total_price AS Загальна_вартість,
    CONCAT(c.city_name, ', ', d.post_office) AS Адреса_доставки,
    (e.employee_name + ' ' + e.employee_surname) AS Працівник
FROM Orders o
JOIN Users u ON o.user_id = u.user_id
LEFT JOIN Employees e ON o.employee_id = e.employee_id
LEFT JOIN DeliveryAddresses d ON o.delivery_address_id = d.delivery_address_id
LEFT JOIN Cities c ON d.city_id = c.city_id
WHERE 1=1";

            if (!string.IsNullOrEmpty(selectedStatus))
            {
                queryString += " AND o.order_status = @selectedStatus";
            }

            if (orderMinPrice.HasValue)
            {
                queryString += " AND o.total_price >= @orderMinPrice";
            }

            if (orderMaxPrice.HasValue)
            {
                queryString += " AND o.total_price <= @orderMaxPrice";
            }

            if (startDate.HasValue)
            {
                queryString += " AND o.order_date >= @startDate";
            }

            if (endDate.HasValue)
            {
                queryString += " AND o.order_date <= @endDate";
            }
            queryString += " ORDER BY o.order_date DESC;";
            using (SqlCommand cmd = new SqlCommand(queryString, connection))
            {
                if (!string.IsNullOrEmpty(selectedStatus))
                {
                    cmd.Parameters.AddWithValue("@selectedStatus", selectedStatus);
                }

                if (orderMinPrice.HasValue)
                {
                    cmd.Parameters.AddWithValue("@orderMinPrice", orderMinPrice.Value);
                }

                if (orderMaxPrice.HasValue)
                {
                    cmd.Parameters.AddWithValue("@orderMaxPrice", orderMaxPrice.Value);
                }

                if (startDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate.Value);
                }

                if (endDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("@endDate", endDate.Value);
                }
               
                DataTable dataTable = new DataTable();
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd))
                {
                    dataAdapter.Fill(dataTable);
                }

                return dataTable;
            }
        }
        private void заЗамовчуваннямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ApplyFilters(dataBase.getSqlConnection(), selectedStatus, orderMinPrice, orderMaxPrice, startDate, endDate);
        }

        private void датаЗаСпаданнямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = SortByDate(dataBase.getSqlConnection(), selectedStatus, orderMinPrice, orderMaxPrice, startDate, endDate);
        }

        private void цінаЗаЗростаннямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = SortByPrice(dataBase.getSqlConnection(), selectedStatus, orderMinPrice, orderMaxPrice, startDate, endDate);

        }
    }
    }

