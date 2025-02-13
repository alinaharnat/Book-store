using Bookstore.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;


namespace Bookstore.Forms
{
    public partial class UserOrdersForm : Form
    {
        public Cart UserCart {  get; set; } 
        private int userId;
        DataBase dataBase = new DataBase();
        public UserOrdersForm()
        {
            InitializeComponent();
        }
        public UserOrdersForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void UserOrdersForm_Load(object sender, EventArgs e)
        {
            Order.RefreshDataGridForUser(dataGridView1, dataBase.getSqlConnection(), userId);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var connection = dataBase.getSqlConnection();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                using (connection)
                {
                    connection.Open();
                    DataTable orderItems = Item.GetOrderItems(connection, orderId, dataGridView2);

                    dataGridView2.DataSource = orderItems;
                    dataGridView2.Columns["ID"].Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Оберіть замовлення для перегляду позицій.");
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) 
            {
                int orderId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
                string orderStatus = dataGridView1.SelectedRows[0].Cells["Статус"].Value.ToString();
                if(orderStatus != "Нове")
                {
                    MessageBox.Show("Скасувати замовлення з таким статусом не можливо.");
                }

                else
                {
                    var confirmResult = MessageBox.Show(
                    "Ви впевнені, що хочете скасувати це замовлення?",
                    "Підтвердження видалення",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                    if (confirmResult == DialogResult.Yes)
                    {
                        //var connection = dataBase.getSqlConnection();
                        var list = Item.GetItemIdsForOrder(orderId, dataBase.getSqlConnection());
                        Item.ProcessItems(list, dataBase.getSqlConnection());
                        Order.DeleteOrder(orderId, dataBase.getSqlConnection());
                        Order.RefreshDataGridForUser(dataGridView1,dataBase.getSqlConnection(), userId);

                    }
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, оберіть замовлення для видалення.", "Помилка", MessageBoxButtons.OK);
            }
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void printBillPictureBox_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                var userInfo = GetUserInfoByUserId(userId, dataBase.getSqlConnection());
                string userName = userInfo.fullName;
                string phone = userInfo.phoneNumber.ToString();

                string address = dataGridView1.SelectedRows[0].Cells["Адреса доставки"].Value.ToString();
             
                var dateValue = dataGridView1.SelectedRows[0].Cells["Дата"].Value;
                string date = "";
                if (dateValue != null && DateTime.TryParse(dateValue.ToString(), out DateTime orderDate))
                {
                    date = $"{orderDate:dd.MM.yyyy}  ";

                    
                }
                var payment = dataGridView1.SelectedRows[0].Cells["Метод_оплати"].Value.ToString();
                string checkContent = FormatCheck(orderId,userName,phone,date,address,payment);
                SaveCheckToFile(checkContent);
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть замовлення для друку чека.");
            }
           
        }
        private string FormatCheck(int orderId,string userName,string phoneNumber, string orderDate, string address, string payment)
        {
            StringBuilder checkBuilder = new StringBuilder();

            decimal totalAmount = 0m;

            checkBuilder.AppendLine("Bookstore");
            checkBuilder.AppendLine();
            checkBuilder.AppendLine($"Замовлення #{orderId}");
            checkBuilder.AppendLine();
            checkBuilder.AppendLine($"{orderDate:dd-MM-yyyy}");
            checkBuilder.AppendLine();
            checkBuilder.AppendLine($"{userName} тел.:{phoneNumber}");
            checkBuilder.AppendLine();
            checkBuilder.AppendLine($"{address} ");
            checkBuilder.AppendLine();
            checkBuilder.AppendLine($"Метод оплати:{payment} ");
            checkBuilder.AppendLine("-----------------------------------------------------------");

            checkBuilder.AppendLine("Товар                К-ть     Ціна        Сума");
            checkBuilder.AppendLine("-----------------------------------------------------------");

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.IsNewRow) continue;

                string productName = row.Cells["Назва книги"].Value.ToString();
                int quantity = Convert.ToInt32(row.Cells["Кількість"].Value);
                decimal price = Convert.ToDecimal(row.Cells["Ціна за одиницю"].Value);
                decimal total = quantity * price;

                totalAmount += total;

                checkBuilder.AppendLine($"{productName,-18}{quantity,6}{price,12:F2}{total,12:F2}");
            }

            checkBuilder.AppendLine("-----------------------------------------------------------");
            checkBuilder.AppendLine();
            checkBuilder.AppendLine($"Загальна сума:        {totalAmount,12:F2}");
            checkBuilder.AppendLine();
            checkBuilder.AppendLine("-----------------------------------------------------------");
            checkBuilder.AppendLine("Дякуємо за покупку!");

            return checkBuilder.ToString();
        }

        private static void SaveCheckToFile(string checkContent)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
                saveFileDialog.DefaultExt = "txt";  
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    File.WriteAllText(filePath, checkContent, Encoding.UTF8);

                    
                }
            }
        }

        private static (string fullName, string phoneNumber) GetUserInfoByUserId(int userId,SqlConnection connection)
        {
            string fullName = string.Empty;
            string phoneNumber = string.Empty;

            string query = "SELECT user_name, user_surname, user_phone FROM Users WHERE user_id = @UserID";

            using (connection)
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read()) 
                {
                    string firstName = reader["user_name"].ToString();
                    string lastName = reader["user_surname"].ToString();
                    phoneNumber = reader["user_phone"].ToString();
                    fullName = $"{firstName} {lastName}"; 
                }
            }

            return (fullName, phoneNumber);
        }

        private void booksLabel_Click(object sender, EventArgs e)
        {
            var form = new UserMainForm(userId);
            form.UserCart = this.UserCart;
            form.ShowDialog();  
        }
    }
}
