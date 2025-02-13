using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Bookstore.Classes
{
    public class Cart
    {
        private List<Item> items;

        public Cart()
        {
            items = new List<Item>();
        }

        public  void AddItem(int bookId, int quantity,SqlConnection connection)
        {
            var existingItem = items.FirstOrDefault(item => item.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.ItemPrice += GetBookPriceFromDatabase(bookId, connection)* (decimal)quantity;
            }
            else
            {
                var price = GetBookPriceFromDatabase(bookId, connection);
                items.Add(new Item
                {
                  
                    BookId = bookId,
                    BookTitle = GetBookTitleById(bookId, connection),
                    Quantity = quantity,
                    BookPrice = price,
                    ItemPrice = price * quantity,
                });
            }
        }

        public void RemoveItem(int bookId)
        {
            var item = items.FirstOrDefault(i => i.BookId == bookId);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        public void ClearCart()
        {
            items.Clear();
        }

        public List<Item> GetItems()
        {
            return items;
        }
        private void LoadCartItems(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();

            foreach (var item in this.GetItems())
            {
                dataGridView.Rows.Add(item.BookId, item.BookTitle, item.Quantity, item.ItemPrice);
            }
        }
        public void UpdateItemQuantity(int bookId, int newQuantity)
        {
            var item = items.FirstOrDefault(i => i.BookId == bookId);
            if (item != null)
            {
                item.Quantity = newQuantity;
                item.ItemPrice = newQuantity * item.BookPrice; 
            }
        }
        //methods related with saving to db
       
        public static bool SaveOrder(int userId, List<Item> items, decimal totalAmount,string paymentMethod, DateTime orderDate, string orderStatus,  int deliveryAddressId, SqlConnection connection)
        {
            bool result = false;
            using (connection )
            {
                connection.Open();

                int orderId = CreateOrder(connection, userId,paymentMethod, orderDate,orderStatus, totalAmount,deliveryAddressId);

                foreach (var item in items)
                {
                    
                  result = Item.InsertItem(orderId,item.BookId,item.Quantity,connection);
                  if(result == false)
                    {
                        Order.DeleteOrder(orderId, connection);
                        return false;
                    }

                }
            }
            return true;
        }
        private static int CreateOrder(SqlConnection connection, int userId, string paymentMethod, DateTime orderDate, string orderStatus, decimal totalPrice,  int? deliveryAddressId)
        {
            string query = @"
        INSERT INTO Orders (user_id, payment_method, order_date, order_status, total_price, delivery_address_id)
        OUTPUT INSERTED.order_id
        VALUES (@UserId, @PaymentMethod, @OrderDate, @OrderStatus, @TotalPrice,  @DeliveryAddressId)";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                cmd.Parameters.AddWithValue("@OrderStatus", orderStatus);
                cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);

                cmd.Parameters.AddWithValue("@DeliveryAddressId", deliveryAddressId);

                return (int)cmd.ExecuteScalar();
            }
        }
        ///
        private decimal GetBookPriceFromDatabase(int bookId, SqlConnection connection)
        {
            decimal price = 0;

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.ConnectionString = "Data Source=DESKTOP-SEJLSPK; Initial Catalog=BookstoreDB; Integrated Security=True"; 
            }

            string query = "SELECT book_price FROM Books WHERE book_id = @BookId";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);

                connection.Open();
                var result = command.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    price = Convert.ToDecimal(result);
                }
            }

            return price;
        }

        public decimal GetTotalPrice()
        {
            return items.Sum(item => item.ItemPrice);
        }
        public static string GetBookTitleById(int bookId, SqlConnection connection)
        {
            string bookTitle = null;
            string query = "SELECT book_title FROM Books WHERE book_id = @BookId";

            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.ConnectionString = "Data Source=DESKTOP-SEJLSPK; Initial Catalog=BookstoreDB; Integrated Security=True";
                connection.Open();
            }

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BookId", bookId);

                var result = command.ExecuteScalar();

                if (result != null)
                {
                    bookTitle = result.ToString();
                }
            }

            return bookTitle;
        }
    }

}
